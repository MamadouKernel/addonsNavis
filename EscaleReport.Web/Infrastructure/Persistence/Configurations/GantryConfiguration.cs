using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class GantryConfiguration : IEntityTypeConfiguration<Gantry>
{
    public void Configure(EntityTypeBuilder<Gantry> builder)
    {
        builder.Property(g => g.Code).HasMaxLength(20).IsRequired();
        builder.HasIndex(g => g.Code).IsUnique();
        builder.Property(g => g.UpdatedAtUtc).IsConcurrencyToken();
    }
}
