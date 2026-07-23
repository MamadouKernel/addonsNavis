using EscaleReport.Web.Domain.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class AuditLogEntryConfiguration : IEntityTypeConfiguration<AuditLogEntry>
{
    public void Configure(EntityTypeBuilder<AuditLogEntry> builder)
    {
        builder.Property(a => a.Action).IsRequired().HasMaxLength(200);
        builder.Property(a => a.UserName).HasMaxLength(256);
        builder.Property(a => a.Cible).HasMaxLength(200);
        builder.HasIndex(a => a.DateUtc);
        builder.HasIndex(a => a.UserId);
    }
}
