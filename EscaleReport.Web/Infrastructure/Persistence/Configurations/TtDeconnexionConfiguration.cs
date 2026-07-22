using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class TtDeconnexionConfiguration : IEntityTypeConfiguration<TtDeconnexion>
{
    public void Configure(EntityTypeBuilder<TtDeconnexion> builder)
    {
        builder.Ignore(d => d.Duree);
        builder.Ignore(d => d.EstResolue);

        builder.Property(d => d.NumeroTt).HasMaxLength(100).IsRequired();
        builder.Property(d => d.Raison).HasMaxLength(1000);
        builder.Property(d => d.UpdatedAtUtc).IsConcurrencyToken();
    }
}
