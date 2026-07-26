using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Common.Security;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

[Authorize]
public class DashboardController(ICurrentUserService currentUser) : Controller
{
    public IActionResult Index()
    {
        var role = ResolveRole();
        var all = BuildAllowedActions();
        var primaryControllers = role switch
        {
            Roles.Administrateur => new[] { "Users", "Parametrage", "Audit", "Statistics" },
            Roles.VesselPlanner => new[] { "Escales", "Notifications" },
            Roles.Dispatcher => new[] { "Dispatch", "Account" },
            Roles.CargoController => new[] { "Cargo", "Notifications", "Escales" },
            Roles.YardPlanner => new[] { "YardPlanner" },
            Roles.IttController => new[] { "Itt" },
            Roles.CoordinateurControlRoom => new[] { "Coordination", "Notifications", "Reporting" },
            Roles.ShiftManager => new[] { "Reporting", "Statistics" },
            _ => new[] { "Escales" }
        };

        var model = new RoleDashboardViewModel
        {
            RoleLabel = RoleLabel(role),
            Heading = Heading(role),
            Introduction = Introduction(role),
            Poste = currentUser.Poste,
            PrimaryActions = all.Where(x => primaryControllers.Contains(x.Controller)).ToList(),
            OtherActions = all.Where(x => !primaryControllers.Contains(x.Controller)).ToList()
        };
        return View(model);
    }

    private List<RoleDashboardAction> BuildAllowedActions()
    {
        var actions = new List<RoleDashboardAction>();
        void Add(bool allowed, string title, string description, string controller, string action, string icon, string priority = "normal")
        {
            if (allowed) actions.Add(new(title, description, controller, action, icon, priority));
        }

        Add(ModuleAccess.CanViewEscales(currentUser), "Escales", "Consulter et piloter les escales autorisées.", "Escales", "Index", "ship", "high");
        Add(ModuleAccess.CanViewDispatch(currentUser, "STS"), "Dispatch STS", "Affectations, pointeurs et incidents portiques.", "Dispatch", "Sts", "crane", "high");
        Add(ModuleAccess.CanViewDispatch(currentUser, "TT"), "Dispatch TT", "Effectifs, navires et déconnexions tracteurs.", "Dispatch", "Tt", "truck", "high");
        Add(ModuleAccess.CanViewDispatch(currentUser, "RTG"), "Dispatch RTG", "Effectifs, pannes et clashs RTG.", "Dispatch", "Rtg", "grid", "high");
        Add(ModuleAccess.CanViewDispatch(currentUser, "Autres engins"), "Autres engins", "Suivre les effectifs et problèmes matériels.", "Dispatch", "AutresEngins", "tool", "high");
        Add(currentUser.IsInRole(Roles.Dispatcher), "Poste du jour", "Changer rapidement le poste opérationnel actif.", "Account", "ChooseDispatchPost", "switch");
        Add(ModuleAccess.CanViewCargo(currentUser), "Cargo Control", "Contrôler les consommations et écarts cargo.", "Cargo", "Index", "box", "high");
        Add(ModuleAccess.CanViewYard(currentUser), "Yard Planning", "Organiser transferts, housekeeping et plans yard.", "YardPlanner", "Index", "yard", "high");
        Add(ModuleAccess.CanViewItt(currentUser), "Flux ITT", "Suivre les transferts, incidents et équipements.", "Itt", "Index", "transfer", "high");
        Add(ModuleAccess.CanViewCoordination(currentUser), "Coordination", "Superviser les opérations et arbitrer les incidents.", "Coordination", "Index", "radar", "high");
        Add(ModuleAccess.CanViewNotifications(currentUser), "Alertes", "Prioriser les anomalies nécessitant une action.", "Notifications", "Index", "bell", "high");
        Add(ModuleAccess.CanViewReporting(currentUser), "Rapport de shift", "Préparer, contrôler et valider la relève.", "Reporting", "Index", "report", "high");
        Add(ModuleAccess.CanViewStatistics(currentUser), "Indicateurs", "Analyser la performance opérationnelle.", "Statistics", "Index", "chart");
        Add(ModuleAccess.CanViewUsers(currentUser), "Comptes et équipes", "Gérer les rôles, permissions et affectations.", "Users", "Index", "users");
        Add(ModuleAccess.CanViewSettings(currentUser), "Paramétrage", "Administrer les référentiels et seuils.", "Parametrage", "Index", "settings");
        Add(ModuleAccess.CanViewAudit(currentUser), "Journal d’audit", "Contrôler les actions et changements sensibles.", "Audit", "Index", "history");
        return actions;
    }

    private string ResolveRole() => Roles.All.FirstOrDefault(currentUser.IsInRole) ?? "Utilisateur";
    private static string RoleLabel(string role) => role switch
    {
        Roles.VesselPlanner => "Vessel Planner", Roles.CargoController => "Cargo Controller",
        Roles.YardPlanner => "Yard Planner", Roles.IttController => "ITT Controller",
        Roles.CoordinateurControlRoom => "Coordinateur Control Room", Roles.ShiftManager => "Shift Manager",
        _ => role
    };
    private static string Heading(string role) => role switch
    {
        Roles.Administrateur => "Centre de contrôle de l’application",
        Roles.CoordinateurControlRoom => "Supervision des opérations",
        Roles.ShiftManager => "Pilotage du shift",
        Roles.Dispatcher => "Votre poste opérationnel",
        _ => "Votre espace de travail"
    };
    private static string Introduction(string role) => role switch
    {
        Roles.Administrateur => "Accédez à la gouvernance, aux équipes et aux indicateurs tout en gardant la possibilité d’ouvrir chaque module métier.",
        Roles.Dispatcher => "Retrouvez en priorité les écrans du poste sélectionné et changez de poste lorsque l’organisation du shift évolue.",
        Roles.CoordinateurControlRoom => "Concentrez-vous sur les alertes, les incidents transverses et la continuité du shift.",
        Roles.ShiftManager => "Préparez la relève, validez les rapports et suivez les indicateurs utiles à la décision.",
        _ => "Les accès ci-dessous sont adaptés à votre rôle et à vos permissions individuelles."
    };
}
