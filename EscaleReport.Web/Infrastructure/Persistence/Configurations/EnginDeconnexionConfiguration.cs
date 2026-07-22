using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class EnginDeconnexionConfiguration : IEntityTypeConfiguration<EnginDeconnexion>
{
    public void Configure(EntityTypeBuilder<EnginDeconnexion> builder)
    {
        builder.Ignore(d => d.Duree);
        builder.Ignore(d => d.EstResolu);

        builder.Property(d => d.Engin).HasMaxLength(100).IsRequired();
        builder.Property(d => d.Motif).HasMaxLength(1000);
        builder.Property(d => d.UpdatedAtUtc).IsConcurrencyToken();
    }
}
