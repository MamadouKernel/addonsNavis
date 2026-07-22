using EscaleReport.Web.Domain.Itt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class IttTransferConfiguration : IEntityTypeConfiguration<IttTransfer>
{
    public void Configure(EntityTypeBuilder<IttTransfer> builder)
    {
        builder.Ignore(t => t.NombreRestant);
        builder.Ignore(t => t.PourcentageAvancement);

        builder.Property(t => t.NavireConnexion).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Observations).HasMaxLength(1000);
        builder.Property(t => t.UpdatedAtUtc).IsConcurrencyToken();
    }
}
