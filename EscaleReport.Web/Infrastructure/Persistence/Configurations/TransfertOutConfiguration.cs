using EscaleReport.Web.Domain.YardPlanning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class TransfertOutConfiguration : IEntityTypeConfiguration<TransfertOut>
{
    public void Configure(EntityTypeBuilder<TransfertOut> builder)
    {
        builder.Ignore(t => t.EstTermine);

        builder.Property(t => t.Bay).HasMaxLength(50).IsRequired();
        builder.Property(t => t.DetailOuDestination).HasMaxLength(500);
        builder.Property(t => t.Commentaire).HasMaxLength(1000);
        builder.Property(t => t.UpdatedAtUtc).IsConcurrencyToken();
    }
}
