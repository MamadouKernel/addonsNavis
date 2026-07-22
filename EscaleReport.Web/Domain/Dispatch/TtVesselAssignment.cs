using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §7.2 : affectation des TT par navire. L'écart entre besoin et affectation réelle
// devra être signalé — calculé ici plutôt que ressaisi, pour ne jamais diverger de la donnée.
public class TtVesselAssignment : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }
    public int NombrePrevu { get; set; }
    public int NombreAffecte { get; set; }
    public int NombreOperationnel { get; set; }
    public string? Observations { get; set; }

    public int Ecart => NombreAffecte - NombrePrevu;
}
