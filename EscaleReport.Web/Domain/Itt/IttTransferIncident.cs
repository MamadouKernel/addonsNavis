using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Itt;

// CDC §13.2 "Incidents de transfert".
public class IttTransferIncident : BaseAuditableEntity
{
    public string DifficulteOuObjet { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public string? Note { get; set; }
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
