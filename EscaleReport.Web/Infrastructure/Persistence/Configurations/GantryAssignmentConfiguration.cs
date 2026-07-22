using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class GantryAssignmentConfiguration : IEntityTypeConfiguration<GantryAssignment>
{
    public void Configure(EntityTypeBuilder<GantryAssignment> builder)
    {
        builder.Property(a => a.TacheOuZone).HasMaxLength(200);
        builder.Property(a => a.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(a => a.GantryId);
        builder.HasIndex(a => a.EscaleId);

        builder.HasOne<Gantry>().WithMany().HasForeignKey(a => a.GantryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Domain.Escales.Escale>().WithMany().HasForeignKey(a => a.EscaleId).OnDelete(DeleteBehavior.Cascade);
    }
}
