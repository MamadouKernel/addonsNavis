using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class RemplacementOperateurConfiguration : IEntityTypeConfiguration<RemplacementOperateur>
{
    public void Configure(EntityTypeBuilder<RemplacementOperateur> builder)
    {
        builder.Property(r => r.Operateur).HasMaxLength(200).IsRequired();
        builder.Property(r => r.EnginQuitte).HasMaxLength(100).IsRequired();
        builder.Property(r => r.NouvelEngin).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Raison).HasMaxLength(1000);
        builder.Property(r => r.Commentaire).HasMaxLength(1000);
        builder.Property(r => r.UpdatedAtUtc).IsConcurrencyToken();
    }
}
