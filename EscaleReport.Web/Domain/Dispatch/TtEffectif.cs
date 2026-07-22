using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §7.1 : effectif TT du shift. Contrairement au portique (ressource nommée suivie
// individuellement), ici on suit des compteurs agrégés — un seul enregistrement "courant",
// mis à jour par le Dispatcher, avec des valeurs dérivées calculées automatiquement.
public class TtEffectif : BaseAuditableEntity
{
    public int TotalParc { get; set; }
    public int Designes { get; set; }
    public string? RaisonNonDesignation { get; set; }
    public int Retires { get; set; }

    public int NonDesignes => Math.Max(0, TotalParc - Designes);
    public int Disponibles => Math.Max(0, Designes - Retires);
}
