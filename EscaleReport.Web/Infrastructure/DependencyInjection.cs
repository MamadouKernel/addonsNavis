using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Infrastructure.Excel;
using EscaleReport.Web.Infrastructure.Identity;
using EscaleReport.Web.Infrastructure.Pdf;
using EscaleReport.Web.Infrastructure.Persistence;
using EscaleReport.Web.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        // Bascule SqlServer / PostgreSQL pilotée par configuration (appsettings "DatabaseProvider"
        // ou variable d'environnement du même nom), sans recompilation — CDC §19.1 "environnement
        // validé par la DSI". Chaque provider a son propre type de DbContext (et donc son propre
        // historique de migrations) : voir ApplicationDbContext pour le pourquoi.
        var provider = configuration.GetValue<string>("DatabaseProvider") ?? "SqlServer";

        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                // Politique de complexité des mots de passe (CDC §2.4).
                options.Password.RequiredLength = 10;
                options.Password.RequireNonAlphanumeric = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

        if (string.Equals(provider, "Postgres", StringComparison.OrdinalIgnoreCase))
        {
            services.AddDbContext<PostgresApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Postgres")));
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<PostgresApplicationDbContext>());
            identityBuilder.AddEntityFrameworkStores<PostgresApplicationDbContext>();
        }
        else
        {
            services.AddDbContext<SqlServerApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<SqlServerApplicationDbContext>());
            identityBuilder.AddEntityFrameworkStores<SqlServerApplicationDbContext>();
        }

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUserDirectoryService, UserDirectoryService>();
        services.AddScoped<IEscalePdfReportGenerator, QuestPdfEscaleReportGenerator>();
        services.AddScoped<IEscaleExcelReportGenerator, ClosedXmlEscaleExcelReportGenerator>();
        services.AddScoped<IShiftReportPdfGenerator, QuestPdfShiftReportGenerator>();

        return services;
    }
}
