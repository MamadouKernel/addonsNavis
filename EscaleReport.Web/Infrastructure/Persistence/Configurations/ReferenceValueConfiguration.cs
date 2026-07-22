using EscaleReport.Web.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class ReferenceValueConfiguration : IEntityTypeConfiguration<ReferenceValue>
{
    public void Configure(EntityTypeBuilder<ReferenceValue> builder)
    {
        builder.Property(r => r.ListKey).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Value).HasMaxLength(200).IsRequired();

        builder.HasIndex(r => new { r.ListKey, r.Value }).IsUnique();
    }
}
