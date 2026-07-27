using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
namespace EscaleReport.Web.Application.Common.Security;
public static class ModuleAccess
{
    public static bool CanViewEscales(ICurrentUserService u) => HasRole(u, Roles.Administrateur, Roles.VesselPlanner, Roles.Dispatcher, Roles.CargoController, Roles.CoordinateurControlRoom) && u.HasPermission(Permissions.ConsulterEscales);
    public static bool CanViewDispatch(ICurrentUserService u, string poste) => u.IsInRole(Roles.Administrateur) || (u.IsInRole(Roles.Dispatcher) && string.Equals(u.Poste, poste, StringComparison.OrdinalIgnoreCase));
    public static bool CanViewCargo(ICurrentUserService u) => HasRole(u, Roles.Administrateur, Roles.CargoController) && u.HasPermission(Permissions.ConsulterEscales);
    public static bool CanViewYard(ICurrentUserService u) => HasRole(u, Roles.Administrateur, Roles.YardPlanner) && u.HasPermission(Permissions.ConsulterEscales);
    public static bool CanViewItt(ICurrentUserService u) => HasRole(u, Roles.Administrateur, Roles.IttController) && u.HasPermission(Permissions.ConsulterEscales);
    public static bool CanViewCoordination(ICurrentUserService u) => HasRole(u, Roles.Administrateur, Roles.CoordinateurControlRoom) && u.HasPermission(Permissions.ConsulterAutresModules);
    public static bool CanViewNotifications(ICurrentUserService u) => HasRole(u, Roles.Administrateur, Roles.VesselPlanner, Roles.CargoController, Roles.CoordinateurControlRoom) && u.HasPermission(Permissions.ConsulterEscales);
    public static bool CanViewReporting(ICurrentUserService u) => HasRole(u, Roles.Administrateur, Roles.CoordinateurControlRoom, Roles.ShiftManager) && u.HasPermission(Permissions.ConsulterRapportGeneral);
    public static bool CanViewSettings(ICurrentUserService u) => u.IsInRole(Roles.Administrateur) && u.HasPermission(Permissions.ModifierParametres);
    public static bool CanViewUsers(ICurrentUserService u) => u.IsInRole(Roles.Administrateur) && u.HasPermission(Permissions.AdministrerUtilisateurs);
    public static bool CanViewAudit(ICurrentUserService u) => u.IsInRole(Roles.Administrateur) && u.HasPermission(Permissions.ConsulterJournalAudit);
    public static bool CanViewStatistics(ICurrentUserService u) => HasRole(u, Roles.Administrateur, Roles.ShiftManager) && u.HasPermission(Permissions.ConsulterStatistiques);
    private static bool HasRole(ICurrentUserService u, params string[] roles) => roles.Any(u.IsInRole);
}
