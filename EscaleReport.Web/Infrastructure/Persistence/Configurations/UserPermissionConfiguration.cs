using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EscaleReport.Web.Infrastructure.Persistence.Configurations;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.HasKey(p => new { p.UserId, p.PermissionKey });

        builder.Property(p => p.PermissionKey).HasMaxLength(100);

        // Pas de navigation ApplicationUser sur l'entité Domain (voir UserPermission) : la FK
        // est déclarée ici, côté Infrastructure, via le type seul (pas de propriété de navigation
        // requise sur l'entité Domain).
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
