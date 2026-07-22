using EscaleReport.Web.Domain.VesselPlanning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class ContainerAnomalyConfiguration : IEntityTypeConfiguration<ContainerAnomaly>
{
    public void Configure(EntityTypeBuilder<ContainerAnomaly> builder)
    {
        builder.Property(a => a.NumeroConteneur).HasMaxLength(50).IsRequired();
        builder.Property(a => a.LigneMaritime).HasMaxLength(200);
        builder.Property(a => a.Position).HasMaxLength(100);
        builder.Property(a => a.Raison).HasMaxLength(100).IsRequired();
        builder.Property(a => a.ResoluPar).HasMaxLength(256);
        builder.Property(a => a.ReferenceEchange).HasMaxLength(200);
        builder.Property(a => a.Commentaire).HasMaxLength(2000);

        builder.Property(a => a.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(a => a.EscaleId);

        // Pas de navigation Escale sur l'entité (cf. Escale, sans collection de navigation) :
        // la FK est déclarée ici par le type seul, cohérent avec UserPermissionConfiguration.
        builder.HasOne<Domain.Escales.Escale>()
            .WithMany()
            .HasForeignKey(a => a.EscaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
