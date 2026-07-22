using EscaleReport.Web.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.Property(t => t.Cle).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Sujet).HasMaxLength(300).IsRequired();
        builder.Property(t => t.Corps).HasMaxLength(4000).IsRequired();
        builder.Property(t => t.DestinatairesParDefaut).HasMaxLength(1000);
        builder.Property(t => t.UpdatedAtUtc).IsConcurrencyToken();

        builder.HasIndex(t => t.Cle).IsUnique();
    }
}
