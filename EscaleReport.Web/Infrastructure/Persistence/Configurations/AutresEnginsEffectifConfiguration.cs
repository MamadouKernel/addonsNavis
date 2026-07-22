using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class AutresEnginsEffectifConfiguration : IEntityTypeConfiguration<AutresEnginsEffectif>
{
    public void Configure(EntityTypeBuilder<AutresEnginsEffectif> builder)
    {
        builder.Property(e => e.UpdatedAtUtc).IsConcurrencyToken();
    }
}
