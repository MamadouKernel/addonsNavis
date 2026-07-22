using EscaleReport.Web.Domain.Itt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class IttEnginPanneConfiguration : IEntityTypeConfiguration<IttEnginPanne>
{
    public void Configure(EntityTypeBuilder<IttEnginPanne> builder)
    {
        builder.Ignore(p => p.Duree);
        builder.Ignore(p => p.EstResolue);

        builder.Property(p => p.Engin).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Cause).HasMaxLength(1000);
        builder.Property(p => p.ActionRealisee).HasMaxLength(1000);
        builder.Property(p => p.UpdatedAtUtc).IsConcurrencyToken();
    }
}
