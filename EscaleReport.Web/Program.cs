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

// Un déploiement IIS peut tourner en HTTP pur (intranet local, pas de certificat) : forcer
// Secure=Always sur le cookie / HSTS / la redirection HTTPS y casserait le login. Piloté par
// config plutôt que par IsDevelopment() seul, pour que le même binaire publié s'adapte à la
// cible réelle via appsettings.Production.json (RequireHttps=false y est déjà positionné pour
// le serveur IIS local prévu — voir STATUS.md) sans recompilation.
var requireHttps = !builder.Environment.IsDevelopment()
    && builder.Configuration.GetValue("Security:RequireHttps", true);

// Par défaut, Identity ne revalide le security stamp (donc IsActive/mot de passe/rôle) que
// toutes les 30 minutes — un compte désactivé pendant qu'un utilisateur est déjà connecté
// resterait donc utilisable jusqu'à 30 min de plus. Réduit à 2 min : UsersController.ToggleActive
// change le stamp à la désactivation, ce qui invalide le cookie dès la prochaine revalidation.
builder.Services.Configure<Microsoft.AspNetCore.Identity.SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(2);
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8); // CDC §2.4 "expiration des sessions"
    options.Cookie.SecurePolicy = requireHttps
        ? CookieSecurePolicy.Always
        : CookieSecurePolicy.SameAsRequest;
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
// Même rendu sûr en développement et en production : aucune trace technique n'est envoyée
// au navigateur. Les détails restent disponibles dans les journaux avec TraceIdentifier.
app.UseExceptionHandler("/Home/Error");

// Une navigation interrompue est un événement HTTP normal, pas une panne applicative.
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.Clear();
            context.Response.StatusCode = 499; // Client Closed Request (convention de fait).
        }
    }
});

// Uniformise aussi les codes produits sans exception (route inconnue, accès refusé, etc.).
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

if (!app.Environment.IsDevelopment())
{
    if (requireHttps)
    {
        app.UseHsts();
    }
}

if (requireHttps)
{
    app.UseHttpsRedirection();
}

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
