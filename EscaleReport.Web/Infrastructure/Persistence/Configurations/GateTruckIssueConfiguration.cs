using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class GateTruckIssueConfiguration : IEntityTypeConfiguration<GateTruckIssue>
{
    public void Configure(EntityTypeBuilder<GateTruckIssue> builder)
    {
        builder.Ignore(g => g.Duree);
        builder.Ignore(g => g.EstResolu);

        builder.Property(g => g.CamionReference).HasMaxLength(100).IsRequired();
        builder.Property(g => g.ProblemeRencontre).HasMaxLength(1000).IsRequired();
        builder.Property(g => g.ActionRealisee).HasMaxLength(1000);
        builder.Property(g => g.UpdatedAtUtc).IsConcurrencyToken();
    }
}
