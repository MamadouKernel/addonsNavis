using EscaleReport.Web.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class AlertThresholdConfiguration : IEntityTypeConfiguration<AlertThreshold>
{
    public void Configure(EntityTypeBuilder<AlertThreshold> builder)
    {
        builder.Property(t => t.Cle).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Libelle).HasMaxLength(300).IsRequired();
        builder.Property(t => t.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(t => t.Cle).IsUnique();
    }
}
