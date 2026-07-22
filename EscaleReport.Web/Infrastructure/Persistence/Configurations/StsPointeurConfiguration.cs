using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class StsPointeurConfiguration : IEntityTypeConfiguration<StsPointeur>
{
    public void Configure(EntityTypeBuilder<StsPointeur> builder)
    {
        builder.Property(p => p.Nom).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Role).HasMaxLength(100);
        builder.Property(p => p.NavireOuZone).HasMaxLength(200);
        builder.Property(p => p.Remarque).HasMaxLength(1000);
        builder.Property(p => p.UpdatedAtUtc).IsConcurrencyToken();
    }
}
