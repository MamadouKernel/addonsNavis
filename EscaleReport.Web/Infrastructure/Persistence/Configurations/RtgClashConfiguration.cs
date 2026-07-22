using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class RtgClashConfiguration : IEntityTypeConfiguration<RtgClash>
{
    public void Configure(EntityTypeBuilder<RtgClash> builder)
    {
        builder.Ignore(c => c.Duree);
        builder.Ignore(c => c.EstResolu);

        builder.Property(c => c.Lieu).HasMaxLength(200).IsRequired();
        builder.Property(c => c.EnginsConcernes).HasMaxLength(500).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(1000).IsRequired();
        builder.Property(c => c.ActionRealisee).HasMaxLength(1000);
        builder.Property(c => c.UpdatedAtUtc).IsConcurrencyToken();
    }
}
