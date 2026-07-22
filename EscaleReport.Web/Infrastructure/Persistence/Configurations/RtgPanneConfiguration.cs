using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class RtgPanneConfiguration : IEntityTypeConfiguration<RtgPanne>
{
    public void Configure(EntityTypeBuilder<RtgPanne> builder)
    {
        builder.Property(p => p.Engin).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Raison).HasMaxLength(1000);
        builder.Property(p => p.CommentaireReprise).HasMaxLength(1000);
        builder.Ignore(p => p.Duree);
        builder.Ignore(p => p.EstResolue);
        builder.Property(p => p.UpdatedAtUtc).IsConcurrencyToken();
    }
}
