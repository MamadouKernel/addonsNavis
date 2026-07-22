using EscaleReport.Web.Domain.VesselPlanning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class DangerousContainerConfiguration : IEntityTypeConfiguration<DangerousContainer>
{
    public void Configure(EntityTypeBuilder<DangerousContainer> builder)
    {
        builder.Ignore(c => c.AlerteRenouvellement);

        builder.Property(c => c.NumeroConteneur).HasMaxLength(50).IsRequired();
        builder.Property(c => c.LigneMaritime).HasMaxLength(200);
        builder.Property(c => c.ClasseImo).HasMaxLength(50);
        builder.Property(c => c.Position).HasMaxLength(100);
        builder.Property(c => c.Commentaire).HasMaxLength(2000);
        builder.Property(c => c.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(c => c.EscaleId);
        builder.HasOne<Domain.Escales.Escale>().WithMany().HasForeignKey(c => c.EscaleId).OnDelete(DeleteBehavior.Cascade);
    }
}
