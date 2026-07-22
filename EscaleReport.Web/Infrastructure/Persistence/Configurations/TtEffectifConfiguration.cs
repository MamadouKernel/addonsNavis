using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class TtEffectifConfiguration : IEntityTypeConfiguration<TtEffectif>
{
    public void Configure(EntityTypeBuilder<TtEffectif> builder)
    {
        builder.Ignore(t => t.NonDesignes);
        builder.Ignore(t => t.Disponibles);

        builder.Property(t => t.RaisonNonDesignation).HasMaxLength(300);
        builder.Property(t => t.UpdatedAtUtc).IsConcurrencyToken();
    }
}
