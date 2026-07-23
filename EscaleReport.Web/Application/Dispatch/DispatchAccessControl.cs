using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;

namespace EscaleReport.Web.Application.Dispatch;

// CDC §2.2 : le Dispatcher n'agit que sur son poste (STS/TT/RTG/Autres engins). La permission
// SaisirDonneesModule/ModifierDonneesModule autorise la saisie en general mais ne distingue pas
// les postes entre eux ; cette verification ferme ce trou de cloisonnement horizontal.
// L'Administrateur passe toujours (acces complet, CDC §2.2).
public static class DispatchAccessControl
{
    public static bool CanAccessPoste(ICurrentUserService currentUser, string poste) =>
        currentUser.IsInRole(Roles.Administrateur) ||
        (currentUser.IsInRole(Roles.Dispatcher) &&
         string.Equals(currentUser.Poste, poste, StringComparison.OrdinalIgnoreCase));
}
