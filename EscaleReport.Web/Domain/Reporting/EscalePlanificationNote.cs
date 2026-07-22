using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Reporting;

// CDC §14.1 : rubrique "Planification" du rapport de fin de shift — "possibilité d'ajouter un
// commentaire ou une consigne par navire à l'attention de la relève". Un enregistrement par
// escale (mis à jour au fil du shift, pas d'historique).
public class EscalePlanificationNote : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }
    public string? Commentaire { get; set; }
}
