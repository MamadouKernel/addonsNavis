using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

public enum GateOperationType { Livraison = 0, Reception = 1 }

// CDC §8.5 "Problèmes des camions Gate".
public class GateTruckIssue : BaseAuditableEntity
{
    public GateOperationType TypeOperation { get; set; }
    public string CamionReference { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public string ProblemeRencontre { get; set; } = string.Empty;
    public string? ActionRealisee { get; set; }

    public TimeSpan? Duree => DateFinUtc.HasValue ? DateFinUtc.Value - DateDebutUtc : null;
    public bool EstResolu => DateFinUtc.HasValue;

    public void Cloturer(string? actionRealisee)
    {
        DateFinUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(actionRealisee))
        {
            ActionRealisee = actionRealisee;
        }
    }
}
