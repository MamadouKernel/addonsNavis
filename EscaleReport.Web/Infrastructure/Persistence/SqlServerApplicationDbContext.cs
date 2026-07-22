using EscaleReport.Web.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Infrastructure.Persistence;

// Historique de migrations dédié : Infrastructure/Persistence/Migrations/SqlServer.
public class SqlServerApplicationDbContext(
    DbContextOptions<SqlServerApplicationDbContext> options,
    AuditableEntitySaveChangesInterceptor auditInterceptor)
    : ApplicationDbContext(options, auditInterceptor);
