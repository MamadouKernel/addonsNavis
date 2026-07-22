using EscaleReport.Web.Domain.Cargo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class CargoConsommationConfiguration : IEntityTypeConfiguration<CargoConsommation>
{
    public void Configure(EntityTypeBuilder<CargoConsommation> builder)
    {
        builder.Ignore(c => c.DischTotal);
        builder.Ignore(c => c.LoadTotal);
        builder.Ignore(c => c.AlerteDischNonRenseigne);
        builder.Ignore(c => c.AlerteLoadNonRenseigne);
        builder.Ignore(c => c.AlerteRevisedNonRecu);

        builder.Property(c => c.RevisedLoadPar).HasMaxLength(256);
        builder.Property(c => c.RevisedLoadObservations).HasMaxLength(1000);
        builder.Property(c => c.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(c => c.EscaleId).IsUnique();
        builder.HasOne<Domain.Escales.Escale>().WithMany().HasForeignKey(c => c.EscaleId).OnDelete(DeleteBehavior.Cascade);
    }
}
