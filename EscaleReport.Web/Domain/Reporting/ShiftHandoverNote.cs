using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Reporting;

// CDC §14.1 "Rapport de fin de shift" — "les actions en cours" et "les points à transmettre
// au shift suivant" sont des synthèses narratives que le Coordinateur rédige lui-même, pas des
// données dérivées d'un autre module. Un enregistrement par (Date, Shift).
public class ShiftHandoverNote : BaseAuditableEntity
{
    public DateOnly Date { get; set; }
    public string? Shift { get; set; }
    public string? ActionsEnCours { get; set; }
    public string? PointsATransmettre { get; set; }
}
