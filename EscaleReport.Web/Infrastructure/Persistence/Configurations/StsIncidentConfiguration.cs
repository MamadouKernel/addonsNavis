using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class StsIncidentConfiguration : IEntityTypeConfiguration<StsIncident>
{
    public void Configure(EntityTypeBuilder<StsIncident> builder)
    {
        builder.Ignore(i => i.Duree);
        builder.Ignore(i => i.EstResolu);

        builder.Property(i => i.TypeIncident).HasMaxLength(100).IsRequired();
        builder.Property(i => i.Cause).HasMaxLength(1000);
        builder.Property(i => i.ConditionsReprise).HasMaxLength(1000);
        builder.Property(i => i.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(i => i.EscaleId);
        builder.HasOne<Domain.Escales.Escale>().WithMany().HasForeignKey(i => i.EscaleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Gantry>().WithMany().HasForeignKey(i => i.GantryId).OnDelete(DeleteBehavior.Restrict);
    }
}
