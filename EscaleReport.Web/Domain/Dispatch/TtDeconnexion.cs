using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §7.3 "Déconnexions TT".
public class TtDeconnexion : BaseAuditableEntity
{
    public string NumeroTt { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateRetourUtc { get; set; }
    public string? Raison { get; set; }
    public bool RetireEffectif { get; set; }

    public TimeSpan? Duree => DateRetourUtc.HasValue ? DateRetourUtc.Value - DateDebutUtc : null;
    public bool EstResolue => DateRetourUtc.HasValue;

    public void SignalerRetour()
    {
        DateRetourUtc = DateTime.UtcNow;
    }
}
