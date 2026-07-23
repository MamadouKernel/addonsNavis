using EscaleReport.Web.Application.Common.Behaviours;
using EscaleReport.Web.Infrastructure;
using EscaleReport.Web.Infrastructure.Persistence;
using EscaleReport.Web.Web.Filters;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => options.Filters.Add<ConcurrencyExceptionFilter>());

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
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Escales}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
