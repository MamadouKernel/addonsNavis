using EscaleReport.Web.Domain.Reporting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class ShiftHandoverNoteConfiguration : IEntityTypeConfiguration<ShiftHandoverNote>
{
    public void Configure(EntityTypeBuilder<ShiftHandoverNote> builder)
    {
        builder.Property(n => n.Shift).HasMaxLength(100);
        builder.Property(n => n.ActionsEnCours).HasMaxLength(2000);
        builder.Property(n => n.PointsATransmettre).HasMaxLength(2000);
        builder.Property(n => n.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(n => new { n.Date, n.Shift }).IsUnique();
    }
}
