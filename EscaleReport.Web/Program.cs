using System.Threading.RateLimiting;
using EscaleReport.Web.Application.Common.Behaviours;
using EscaleReport.Web.Infrastructure;
using EscaleReport.Web.Infrastructure.Persistence;
using EscaleReport.Web.Web.Filters;
using EscaleReport.Web.Web.Security;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
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

// CDC §2.4 : le verrouillage de compte (5 échecs/15 min, voir Infrastructure/DependencyInjection.cs)
// ne freine que par compte ; cette limite par IP protège contre le bourrage réseau visant
// plusieurs comptes à la fois (credential stuffing).
builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync(
            "Trop de tentatives de connexion. Réessayez dans une minute.", cancellationToken);
    };

    options.AddPolicy("login", context => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        }));
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

// En-têtes de sécurité applicatifs (CDC §27). script-src utilise un nonce par requête (posé ici
// et lu par les vues via Context.GetOrCreateCspNonce()) plutôt que 'unsafe-inline' : un <script>
// injecté par un attaquant n'a pas le nonce et ne s'exécute pas. style-src garde 'unsafe-inline'
// (utilisé par des centaines d'attributs style="" Tailwind — un nonce y serait disproportionné
// et une injection CSS n'exécute pas de JavaScript).
app.Use(async (context, next) =>
{
    var nonce = context.GetOrCreateCspNonce();
    var headers = context.Response.Headers;
    headers["X-Content-Type-Options"] = "nosniff";
    headers["X-Frame-Options"] = "DENY";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers["Permissions-Policy"] = "geolocation=(), camera=(), microphone=()";
    headers["Content-Security-Policy"] =
        $"default-src 'self'; script-src 'self' 'nonce-{nonce}'; style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data:; font-src 'self'; object-src 'none'; base-uri 'self'; " +
        "form-action 'self'; frame-ancestors 'self'";
    await next();
});

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Escales}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
