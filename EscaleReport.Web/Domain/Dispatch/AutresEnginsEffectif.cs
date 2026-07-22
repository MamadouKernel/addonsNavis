using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §9.1 : effectif disponible du shift pour les autres engins — enregistrement "courant"
// unique, même principe que RtgEffectif/TtEffectif. Le nombre "retiré" par catégorie n'est pas
// saisi ici : il est calculé automatiquement à partir des EnginProbleme ouverts avec retrait
// d'effectif (voir GetDispatchAutresEnginsQueryHandler).
public class AutresEnginsEffectif : BaseAuditableEntity
{
    public int DisponibleReachStackers { get; set; }
    public int DisponibleEmptyHandlers { get; set; }
    public int DisponibleAutres { get; set; }
}
