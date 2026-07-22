using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §8.4 "Gestion des clashs" (conflits d'utilisation d'engins dans le yard).
public class RtgClash : BaseAuditableEntity
{
    public string Lieu { get; set; } = string.Empty;
    public string EnginsConcernes { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public string Description { get; set; } = string.Empty;
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
