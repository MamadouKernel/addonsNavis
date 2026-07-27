using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;

namespace EscaleReport.Web.Application.Common.Security;

// Source unique des règles d'affichage de la navigation. Les mêmes groupes de rôles sont
// appliqués par [Authorize] sur les contrôleurs, tandis que les handlers conservent le contrôle
// fin des permissions (lecture, saisie, modification, export, administration).
public static class ModuleAccess
{
    public static bool CanViewEscales(ICurrentUserService user) =>
        HasRole(user, Roles.Administrateur, Roles.VesselPlanner, Roles.Dispatcher,
            Roles.CargoController, Roles.CoordinateurControlRoom)
        && user.HasPermission(Permissions.ConsulterEscales);

    public static bool CanViewDispatch(ICurrentUserService user, string poste) =>
        user.IsInRole(Roles.Administrateur)
        || (user.IsInRole(Roles.Dispatcher)
            && string.Equals(user.Poste, poste, StringComparison.OrdinalIgnoreCase));

    public static bool CanViewCargo(ICurrentUserService user) =>
        HasRole(user, Roles.Administrateur, Roles.CargoController)
        && user.HasPermission(Permissions.ConsulterEscales);

    public static bool CanViewYard(ICurrentUserService user) =>
        HasRole(user, Roles.Administrateur, Roles.YardPlanner)
        && user.HasPermission(Permissions.ConsulterEscales);

    public static bool CanViewItt(ICurrentUserService user) =>
        HasRole(user, Roles.Administrateur, Roles.IttController)
        && user.HasPermission(Permissions.ConsulterEscales);

    public static bool CanViewCoordination(ICurrentUserService user) =>
        HasRole(user, Roles.Administrateur, Roles.CoordinateurControlRoom)
        && user.HasPermission(Permissions.ConsulterAutresModules);

    public static bool CanViewNotifications(ICurrentUserService user) =>
        HasRole(user, Roles.Administrateur, Roles.VesselPlanner, Roles.CargoController,
            Roles.CoordinateurControlRoom)
        && user.HasPermission(Permissions.ConsulterEscales);

    public static bool CanViewReporting(ICurrentUserService user) =>
        HasRole(user, Roles.Administrateur, Roles.CoordinateurControlRoom, Roles.ShiftManager)
        && user.HasPermission(Permissions.ConsulterRapportGeneral);

    public static bool CanViewSettings(ICurrentUserService user) =>
        user.IsInRole(Roles.Administrateur)
        && user.HasPermission(Permissions.ModifierParametres);

    public static bool CanViewUsers(ICurrentUserService user) =>
        user.IsInRole(Roles.Administrateur)
        && user.HasPermission(Permissions.AdministrerUtilisateurs);

    public static bool CanViewAudit(ICurrentUserService user) =>
        user.IsInRole(Roles.Administrateur)
        && user.HasPermission(Permissions.ConsulterJournalAudit);

    public static bool CanViewStatistics(ICurrentUserService user) =>
        HasRole(user, Roles.Administrateur, Roles.ShiftManager)
        && user.HasPermission(Permissions.ConsulterStatistiques);

    private static bool HasRole(ICurrentUserService user, params string[] roles) =>
        roles.Any(user.IsInRole);
}