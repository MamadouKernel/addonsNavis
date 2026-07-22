using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class RopnEntryConfiguration : IEntityTypeConfiguration<RopnEntry>
{
    public void Configure(EntityTypeBuilder<RopnEntry> builder)
    {
        builder.Property(r => r.Nom).HasMaxLength(200).IsRequired();
        builder.Property(r => r.Role).HasMaxLength(100);
        builder.Property(r => r.DifficulteRencontree).HasMaxLength(1000).IsRequired();
        builder.Property(r => r.ActionRealisee).HasMaxLength(1000);
        builder.Property(r => r.Commentaire).HasMaxLength(1000);
        builder.Property(r => r.UpdatedAtUtc).IsConcurrencyToken();
    }
}
