using EscaleReport.Web.Domain.Escales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class ReportEmailLogConfiguration : IEntityTypeConfiguration<ReportEmailLog>
{
    public void Configure(EntityTypeBuilder<ReportEmailLog> builder)
    {
        builder.Property(r => r.ReportType).HasMaxLength(100).IsRequired();
        builder.Property(r => r.SentBy).HasMaxLength(256).IsRequired();
        builder.Property(r => r.Recipients).HasMaxLength(1000).IsRequired();
        builder.Property(r => r.Subject).HasMaxLength(300);

        builder.HasIndex(r => r.EscaleId);

        builder.HasOne<Escale>().WithMany().HasForeignKey(r => r.EscaleId).OnDelete(DeleteBehavior.Cascade);
    }
}
