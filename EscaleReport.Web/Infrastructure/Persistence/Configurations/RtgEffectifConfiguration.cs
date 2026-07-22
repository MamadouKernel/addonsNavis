using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class RtgEffectifConfiguration : IEntityTypeConfiguration<RtgEffectif>
{
    public void Configure(EntityTypeBuilder<RtgEffectif> builder)
    {
        builder.Property(r => r.UpdatedAtUtc).IsConcurrencyToken();
    }
}
