using System.Security.Claims;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EscaleReport.Web.Infrastructure.Identity;

// Convertit les permissions accordées (table UserPermission) en claims au moment de la
// connexion, pour que CurrentUserService puisse les lire sans requête BDD à chaque contrôle.
// L'administrateur reçoit toutes les permissions (CDC §2.2 : accès complet par construction).
// Dépend de l'abstraction IApplicationDbContext (pas d'un DbContext concret) : que le provider
// actif soit SqlServerApplicationDbContext ou PostgresApplicationDbContext ne change rien ici.
public class AppUserClaimsPrincipalFactory(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IOptions<IdentityOptions> options,
    IApplicationDbContext dbContext)
    : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<Guid>>(userManager, roleManager, options)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var isAdmin = await UserManager.IsInRoleAsync(user, Roles.Administrateur)
            || await UserManager.IsInRoleAsync(user, Roles.AdministrateurIT);

        var permissionKeys = isAdmin
            ? Permissions.All
            : await dbContext.UserPermissions
                .Where(p => p.UserId == user.Id)
                .Select(p => p.PermissionKey)
                .ToListAsync();

        identity.AddClaims(permissionKeys.Select(k => new Claim(AppClaimTypes.Permission, k)));

        if (!string.IsNullOrWhiteSpace(user.PosteParDefaut))
        {
            identity.AddClaim(new Claim(AppClaimTypes.Poste, user.PosteParDefaut));
        }

        return identity;
    }
}
