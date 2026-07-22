using EscaleReport.Web.Domain.VesselPlanning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class EmptyContainerTargetConfiguration : IEntityTypeConfiguration<EmptyContainerTarget>
{
    public void Configure(EntityTypeBuilder<EmptyContainerTarget> builder)
    {
        builder.Ignore(t => t.QuantiteRestante);

        builder.Property(t => t.LigneMaritime).HasMaxLength(200).IsRequired();
        builder.Property(t => t.TypeConteneur).HasMaxLength(50).IsRequired();
        builder.Property(t => t.MotifCoupure).HasMaxLength(200);
        builder.Property(t => t.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(t => t.EscaleId);
        builder.HasOne<Domain.Escales.Escale>().WithMany().HasForeignKey(t => t.EscaleId).OnDelete(DeleteBehavior.Cascade);
    }
}
