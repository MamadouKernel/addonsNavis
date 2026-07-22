using EscaleReport.Web.Domain.Escales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class EscaleConfiguration : IEntityTypeConfiguration<Escale>
{
    public void Configure(EntityTypeBuilder<Escale> builder)
    {
        builder.Property(e => e.Navire).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Voyage).HasMaxLength(100);
        builder.Property(e => e.LigneMaritime).HasMaxLength(200);
        builder.Property(e => e.VesselVisit).HasMaxLength(100);
        builder.Property(e => e.Quai).HasMaxLength(100);
        builder.Property(e => e.Shift).HasMaxLength(50);
        builder.Property(e => e.Planificateur).HasMaxLength(200);

        // Aide au contrôle d'unicité côté application (CDC §18.1) : Vessel Visit ou
        // combinaison navire/voyage/ETA. Index non-unique : le doublon probable est
        // signalé à l'utilisateur, pas bloqué en base (un même navire peut légitimement
        // revenir avec un nouveau voyage).
        builder.HasIndex(e => e.VesselVisit);
        builder.HasIndex(e => new { e.Navire, e.Voyage, e.Eta });

        // Concurrence optimiste portable SqlServer/Postgres (CDC §18.2) : EF Core compare
        // la valeur originale d'UpdatedAtUtc dans la clause WHERE du UPDATE plutôt que de
        // s'appuyer sur un type propriétaire (rowversion SQL Server / xmin Postgres).
        builder.Property(e => e.UpdatedAtUtc).IsConcurrencyToken();
    }
}
