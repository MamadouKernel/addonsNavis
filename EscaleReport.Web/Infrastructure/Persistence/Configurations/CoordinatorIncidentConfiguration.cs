using EscaleReport.Web.Domain.Coordination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class CoordinatorIncidentConfiguration : IEntityTypeConfiguration<CoordinatorIncident>
{
    public void Configure(EntityTypeBuilder<CoordinatorIncident> builder)
    {
        builder.Ignore(i => i.Duree);
        builder.Ignore(i => i.EstResolu);

        builder.Property(i => i.Objet).HasMaxLength(200).IsRequired();
        builder.Property(i => i.Note).HasMaxLength(1000);
        builder.Property(i => i.ActionRealisee).HasMaxLength(1000);
        builder.Property(i => i.UpdatedAtUtc).IsConcurrencyToken();
    }
}
