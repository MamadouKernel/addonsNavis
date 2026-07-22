using EscaleReport.Web.Domain.Itt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class IttTransferIncidentConfiguration : IEntityTypeConfiguration<IttTransferIncident>
{
    public void Configure(EntityTypeBuilder<IttTransferIncident> builder)
    {
        builder.Ignore(i => i.Duree);
        builder.Ignore(i => i.EstResolu);

        builder.Property(i => i.DifficulteOuObjet).HasMaxLength(500).IsRequired();
        builder.Property(i => i.Note).HasMaxLength(1000);
        builder.Property(i => i.ActionRealisee).HasMaxLength(1000);
        builder.Property(i => i.UpdatedAtUtc).IsConcurrencyToken();
    }
}
