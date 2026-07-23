using EscaleReport.Web.Application.Coordination.Queries.GetCoordinatorDashboard;
using EscaleReport.Web.Domain.Escales;

namespace EscaleReport.Web.Application.Reporting.Queries.GetShiftReport;

// CDC §14.1 "Rapport de fin de shift" — réutilise les mêmes classes de synthèse par poste
// que le tableau de bord Coordinateur (§12), pour ne jamais diverger entre les deux vues.
public class ShiftReportDto
{
    public DateOnly Date { get; set; }
    public string? Shift { get; set; }
    public string Coordinateur { get; set; } = string.Empty;
    public bool NavireCourantMode { get; set; }

    public IReadOnlyList<NavireEtatDto> NaviresPresentsOuAttendus { get; set; } = [];

    public StsSyntheseDto SyntheseSts { get; set; } = new();
    public TtSyntheseDto SyntheseTt { get; set; } = new();
    public RtgAutresEnginsSyntheseDto SyntheseRtgAutresEngins { get; set; } = new();
    public IReadOnlyList<CargoSyntheseDto> SyntheseCargo { get; set; } = [];
    public YardSyntheseDto SyntheseYard { get; set; } = new();
    public IttSyntheseDto SyntheseItt { get; set; } = new();

    // "Les pannes et indisponibilités" / "Les incidents" (§14.1) — vue transverse regroupant
    // tous les postes, au-delà des compteurs déjà présents dans les synthèses par poste.
    public IReadOnlyList<string> PannesEtIndisponibilites { get; set; } = [];
    public IReadOnlyList<string> Incidents { get; set; } = [];

    public string? ActionsEnCours { get; set; }
    public string? PointsATransmettre { get; set; }

    // CDC §2 "Shift Manager" : validation/commentaire de supervision sur le rapport de shift.
    public string? ValidePar { get; set; }
    public DateTime? ValideLeUtc { get; set; }
    public string? CommentaireValidation { get; set; }

    // CDC §18 "Transmission entre shifts" : confirmation de prise de connaissance par le shift entrant.
    public string? PriseDeConnaissanceParUtilisateur { get; set; }
    public DateTime? PriseDeConnaissanceLeUtc { get; set; }

    public IReadOnlyList<PlanificationNavireDto> Planification { get; set; } = [];

    public IReadOnlyList<EscaleOptionDto> EscalesDisponibles { get; set; } = [];
    public IReadOnlyList<string> ShiftsDisponibles { get; set; } = [];
}

public class EscaleOptionDto
{
    public Guid Id { get; set; }
    public string Navire { get; set; } = string.Empty;
}

public class NavireEtatDto
{
    public Guid Id { get; set; }
    public string Navire { get; set; } = string.Empty;
    public string Voyage { get; set; } = string.Empty;
    public string? Quai { get; set; }
    public DateTime Eta { get; set; }
    public StatutOperations StatutOperations { get; set; }
}

public class PlanificationNavireDto
{
    public Guid EscaleId { get; set; }
    public string Navire { get; set; } = string.Empty;
    public string Voyage { get; set; } = string.Empty;
    public string? Quai { get; set; }
    public DateTime Eta { get; set; }
    public StatutPlanification StatutPlanification { get; set; }
    public string? Commentaire { get; set; }
}
