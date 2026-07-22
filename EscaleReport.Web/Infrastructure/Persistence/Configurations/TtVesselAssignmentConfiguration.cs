using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class TtVesselAssignmentConfiguration : IEntityTypeConfiguration<TtVesselAssignment>
{
    public void Configure(EntityTypeBuilder<TtVesselAssignment> builder)
    {
        builder.Ignore(t => t.Ecart);

        builder.Property(t => t.Observations).HasMaxLength(1000);
        builder.Property(t => t.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(t => t.EscaleId);
        builder.HasOne<Domain.Escales.Escale>().WithMany().HasForeignKey(t => t.EscaleId).OnDelete(DeleteBehavior.Cascade);
    }
}
