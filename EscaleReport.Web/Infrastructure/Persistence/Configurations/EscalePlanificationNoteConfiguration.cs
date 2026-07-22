using EscaleReport.Web.Domain.Reporting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class EscalePlanificationNoteConfiguration : IEntityTypeConfiguration<EscalePlanificationNote>
{
    public void Configure(EntityTypeBuilder<EscalePlanificationNote> builder)
    {
        builder.Property(n => n.Commentaire).HasMaxLength(1000);
        builder.Property(n => n.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(n => n.EscaleId).IsUnique();
        builder.HasOne<Domain.Escales.Escale>().WithMany().HasForeignKey(n => n.EscaleId).OnDelete(DeleteBehavior.Cascade);
    }
}
