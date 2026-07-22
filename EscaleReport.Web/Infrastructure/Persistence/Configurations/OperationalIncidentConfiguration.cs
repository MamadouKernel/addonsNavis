using EscaleReport.Web.Domain.VesselPlanning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class OperationalIncidentConfiguration : IEntityTypeConfiguration<OperationalIncident>
{
    public void Configure(EntityTypeBuilder<OperationalIncident> builder)
    {
        builder.Ignore(i => i.Duree);

        builder.Property(i => i.Categorie).HasMaxLength(100).IsRequired();
        builder.Property(i => i.Localisation).HasMaxLength(200);
        builder.Property(i => i.Description).HasMaxLength(2000);
        builder.Property(i => i.ActionRealisee).HasMaxLength(2000);
        builder.Property(i => i.DeclarePar).HasMaxLength(256);
        builder.Property(i => i.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(i => i.EscaleId);
        builder.HasOne<Domain.Escales.Escale>().WithMany().HasForeignKey(i => i.EscaleId).OnDelete(DeleteBehavior.Cascade);
    }
}
