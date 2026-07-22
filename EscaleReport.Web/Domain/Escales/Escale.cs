using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Escales;

// Cf. CDC §4 "Module de gestion des escales" — point d'entrée commun à tous les postes.
public class Escale : BaseAuditableEntity
{
    // Champs obligatoires (CDC §4.2)
    public string Navire { get; set; } = string.Empty;
    public string Voyage { get; set; } = string.Empty;
    public string LigneMaritime { get; set; } = string.Empty;
    public DateTime Eta { get; set; }

    // Champs complémentaires
    public string? VesselVisit { get; set; }
    public string? Quai { get; set; }
    public DateTime? Ata { get; set; }
    public DateTime? Etc { get; set; }
    public string? Shift { get; set; }
    public StatutOperations StatutOperations { get; set; } = StatutOperations.PasEncoreDebutees;
    public StatutPlanification StatutPlanification { get; set; } = StatutPlanification.NonPlanifie;
    public string? Planificateur { get; set; }

    // Une escale incomplète reste un brouillon (CDC §4.2) tant que les champs obligatoires manquent.
    public bool IsDraft { get; set; } = true;

    public bool HasRequiredFields() =>
        !string.IsNullOrWhiteSpace(Navire)
        && !string.IsNullOrWhiteSpace(Voyage)
        && !string.IsNullOrWhiteSpace(LigneMaritime)
        && Eta != default;

    public void RefreshDraftState() => IsDraft = !HasRequiredFields();

    // CDC §4.3 : le passage au statut "Terminées" est soumis à une permission dédiée
    // (Permissions.MarquerEscaleTerminee), contrôlée dans le handler, pas ici.
    public void ChangerStatutOperations(StatutOperations nouveauStatut) => StatutOperations = nouveauStatut;

    public void ChangerStatutPlanification(StatutPlanification nouveauStatut) => StatutPlanification = nouveauStatut;
}
