using System.Security.Claims;
using EscaleReport.Web.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EscaleReport.Web.Infrastructure.Identity;

// Les permissions sont lues depuis les claims du cookie d'authentification (posées à la
// connexion par AppUserClaimsPrincipalFactory), jamais depuis un état renvoyé par le client :
// c'est ce qui garantit le contrôle serveur exigé par le CDC §27.
public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var id = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var guid) ? guid : null;
        }
    }

    public string? UserName => User?.Identity?.Name;

    public bool HasPermission(string permissionKey) =>
        User?.HasClaim(AppClaimTypes.Permission, permissionKey) ?? false;

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}

public static class AppClaimTypes
{
    public const string Permission = "permission";
}
