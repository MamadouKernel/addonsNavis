using EscaleReport.Web.Domain.Dispatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class EnginProblemeConfiguration : IEntityTypeConfiguration<EnginProbleme>
{
    public void Configure(EntityTypeBuilder<EnginProbleme> builder)
    {
        builder.Ignore(p => p.Duree);
        builder.Ignore(p => p.EstResolu);

        builder.Property(p => p.Engin).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Probleme).HasMaxLength(1000).IsRequired();
        builder.Property(p => p.ActionRealisee).HasMaxLength(1000);
        builder.Property(p => p.UpdatedAtUtc).IsConcurrencyToken();
    }
}
