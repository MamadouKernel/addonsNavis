using EscaleReport.Web.Domain.YardPlanning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class VesselYardPlanConfiguration : IEntityTypeConfiguration<VesselYardPlan>
{
    public void Configure(EntityTypeBuilder<VesselYardPlan> builder)
    {
        builder.Property(p => p.ServiceMaritime).HasMaxLength(200).IsRequired();
        builder.Property(p => p.ZoneDebarquement).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Observations).HasMaxLength(1000);
        builder.Property(p => p.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(p => p.EscaleId);
        builder.HasOne<Domain.Escales.Escale>().WithMany().HasForeignKey(p => p.EscaleId).OnDelete(DeleteBehavior.Cascade);
    }
}
