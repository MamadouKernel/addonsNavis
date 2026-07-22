using EscaleReport.Web.Domain.YardPlanning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class HousekeepingTaskConfiguration : IEntityTypeConfiguration<HousekeepingTask>
{
    public void Configure(EntityTypeBuilder<HousekeepingTask> builder)
    {
        builder.Property(t => t.Description).HasMaxLength(1000).IsRequired();
        builder.Property(t => t.Zone).HasMaxLength(100);
        builder.Property(t => t.Priorite).HasMaxLength(50);
        builder.Property(t => t.Responsable).HasMaxLength(200);
        builder.Property(t => t.Note).HasMaxLength(1000);
        builder.Property(t => t.UpdatedAtUtc).IsConcurrencyToken();
    }
}
