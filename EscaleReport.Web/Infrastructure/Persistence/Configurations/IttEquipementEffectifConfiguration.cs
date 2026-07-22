using EscaleReport.Web.Domain.Itt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class IttEquipementEffectifConfiguration : IEntityTypeConfiguration<IttEquipementEffectif>
{
    public void Configure(EntityTypeBuilder<IttEquipementEffectif> builder)
    {
        builder.Property(e => e.Observations).HasMaxLength(1000);
        builder.Property(e => e.UpdatedAtUtc).IsConcurrencyToken();
    }
}
