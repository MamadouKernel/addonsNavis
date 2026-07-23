using EscaleReport.Web.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class CoordinatorModuleVisibilityConfiguration : IEntityTypeConfiguration<CoordinatorModuleVisibility>
{
    public void Configure(EntityTypeBuilder<CoordinatorModuleVisibility> builder)
    {
        builder.Property(m => m.Cle).HasMaxLength(100).IsRequired();
        builder.Property(m => m.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(m => m.Cle).IsUnique();
    }
}
