using EscaleReport.Web.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class GeneralSettingsConfiguration : IEntityTypeConfiguration<GeneralSettings>
{
    public void Configure(EntityTypeBuilder<GeneralSettings> builder)
    {
        builder.Property(s => s.NomSociete).HasMaxLength(200);
        builder.Property(s => s.PlanificateurParDefaut).HasMaxLength(200);
        builder.Property(s => s.UpdatedAtUtc).IsConcurrencyToken();
    }
}
