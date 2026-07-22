using EscaleReport.Web.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Infrastructure.Persistence;

// Historique de migrations dédié : Infrastructure/Persistence/Migrations/Postgres.
public class PostgresApplicationDbContext(
    DbContextOptions<PostgresApplicationDbContext> options,
    AuditableEntitySaveChangesInterceptor auditInterceptor)
    : ApplicationDbContext(options, auditInterceptor);
