using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Itt;

// CDC §13.4 "Pannes des engins de transfert".
public class IttEnginPanne : BaseAuditableEntity
{
    public string Engin { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public string? Cause { get; set; }
    public bool RetireEffectif { get; set; }
    public string? ActionRealisee { get; set; }

    public TimeSpan? Duree => DateFinUtc.HasValue ? DateFinUtc.Value - DateDebutUtc : null;
    public bool EstResolue => DateFinUtc.HasValue;

    public void Cloturer(string? actionRealisee)
    {
        DateFinUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(actionRealisee))
        {
            ActionRealisee = actionRealisee;
        }
    }
}
