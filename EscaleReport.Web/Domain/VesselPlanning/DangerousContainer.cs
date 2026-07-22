using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.VesselPlanning;

public enum BadtStatus { NonPris = 0, Pris = 1, ARenouveler = 2 }
public enum DangerousContainerStatus { ASuivre = 0, Cloture = 1, Reembarque = 2 }

// CDC §5.5 "Gestion des conteneurs dangereux".
public class DangerousContainer : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }

    public string NumeroConteneur { get; set; } = string.Empty;
    public string? LigneMaritime { get; set; }
    public string? ClasseImo { get; set; }
    public string? Position { get; set; }
    public BadtStatus StatutBadt { get; set; } = BadtStatus.NonPris;
    public DateTime? DateValiditeBadt { get; set; }
    public DangerousContainerStatus StatutOperationnel { get; set; } = DangerousContainerStatus.ASuivre;
    public string? Commentaire { get; set; }

    // CDC : le statut « à renouveler » doit être mis en évidence — dérivé, pas ressaisi.
    public bool AlerteRenouvellement => StatutBadt == BadtStatus.ARenouveler;
}
