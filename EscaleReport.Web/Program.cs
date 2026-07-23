using EscaleReport.Web.Application.Common.Behaviours;
using EscaleReport.Web.Infrastructure;
using EscaleReport.Web.Infrastructure.Persistence;
using EscaleReport.Web.Web.Filters;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ConcurrencyExceptionFilter>();
    options.Filters.Add<AppExceptionFilter>();
});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditLoggingBehaviour<,>));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8); // CDC §2.4 "expiration des sessions"
    // SameAsRequest en Dev : pas d'endpoint https dans launchSettings, Always casserait le login local.
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
});

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// Migration auto + seed réservés au Dev : jamais en Production (CDC §27 — pas de raccourci
// qui contourne la validation DSI/recette avant mise à jour d'une base de production).
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var provider = app.Configuration.GetValue<string>("DatabaseProvider") ?? "SqlServer";

    if (string.Equals(provider, "Postgres", StringComparison.OrdinalIgnoreCase))
    {
        await services.GetRequiredService<PostgresApplicationDbContext>().Database.MigrateAsync();
    }
    else
    {
        await services.GetRequiredService<SqlServerApplicationDbContext>().Database.MigrateAsync();
    }

    await DbSeeder.SeedAsync(services, app.Configuration, services.GetRequiredService<ILogger<Program>>());
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// En-têtes de sécurité applicatifs (CDC §27). CSP autorise 'unsafe-inline' pour script/style :
// l'app s'appuie sur des styles/scripts inline dans les vues Razor (pas de nonce en place) ;
// le reste de la politique (pas de sources externes, pas d'iframe, pas d'objets) reste utile.
app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers["X-Content-Type-Options"] = "nosniff";
    headers["X-Frame-Options"] = "DENY";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers["Permissions-Policy"] = "geolocation=(), camera=(), microphone=()";
    headers["Content-Security-Policy"] =
        "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data:; font-src 'self'; object-src 'none'; base-uri 'self'; " +
        "form-action 'self'; frame-ancestors 'self'";
    await next();
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Escales}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
