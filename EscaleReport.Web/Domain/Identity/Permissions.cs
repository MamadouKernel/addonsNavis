namespace EscaleReport.Web.Domain.Identity;

// Catalogue des permissions applicatives (CDC §2.3). Paramétrables par utilisateur
// indépendamment de son rôle principal — voir UserPermission.
public static class Permissions
{
    public const string CreerEscale = "escales.creer";
    public const string ModifierEscale = "escales.modifier";
    public const string SupprimerEscale = "escales.supprimer";
    public const string MarquerEscaleTerminee = "escales.marquer_terminee";
    public const string ConsulterEscales = "escales.consulter";
    public const string SaisirDonneesModule = "module.saisir";
    public const string ModifierDonneesModule = "module.modifier";
    public const string ConsulterAutresModules = "module.consulter_autres";
    public const string GenererPdf = "rapports.generer_pdf";
    public const string GenererExcel = "rapports.generer_excel";
    public const string EnvoyerRapportEmail = "rapports.envoyer_email";
    public const string ConsulterRapportGeneral = "rapports.consulter_general";
    public const string ValiderRapport = "rapports.valider";
    public const string AdministrerUtilisateurs = "admin.utilisateurs";
    public const string ConsulterJournalAudit = "admin.audit";
    public const string ModifierParametres = "admin.parametres";
    public const string ExporterDonnees = "donnees.exporter";
    public const string ConsulterStatistiques = "donnees.statistiques";

    public static readonly IReadOnlyList<string> All = new[]
    {
        CreerEscale, ModifierEscale, SupprimerEscale, MarquerEscaleTerminee, ConsulterEscales,
        SaisirDonneesModule, ModifierDonneesModule, ConsulterAutresModules,
        GenererPdf, GenererExcel, EnvoyerRapportEmail, ConsulterRapportGeneral, ValiderRapport,
        AdministrerUtilisateurs, ConsulterJournalAudit, ModifierParametres,
        ExporterDonnees, ConsulterStatistiques
    };
}

// Rôles principaux prévus au CDC §2.2 (au-delà du rôle Admin transverse "Administrator" d'Identity).
public static class Roles
{
    public const string Administrateur = "Administrateur";
    public const string VesselPlanner = "VesselPlanner";
    public const string Dispatcher = "Dispatcher";
    public const string CargoController = "CargoController";
    public const string YardPlanner = "YardPlanner";
    public const string IttController = "IttController";
    public const string CoordinateurControlRoom = "CoordinateurControlRoom";
    public const string ShiftManager = "ShiftManager";

    public static readonly IReadOnlyList<string> All = new[]
    {
        Administrateur, VesselPlanner, Dispatcher, CargoController,
        YardPlanner, IttController, CoordinateurControlRoom, ShiftManager
    };
}
