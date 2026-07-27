using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §6.4 "Gestion des pointeurs".
public class StsPointeur : BaseAuditableEntity
{
    public string Nom { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string? NavireOuZone { get; set; }
    public DateTime HeurePriseDePosteUtc { get; set; }
    public DateTime? HeureFinUtc { get; set; }
    public string? Remarque { get; set; }
    public TimeSpan? Duree => HeureFinUtc.HasValue ? HeureFinUtc.Value - HeurePriseDePosteUtc : null;

    public void TerminerService(DateTime? heureFinUtc = null)
    {
        var fin = heureFinUtc ?? DateTime.UtcNow;
        HeureFinUtc = fin < HeurePriseDePosteUtc ? HeurePriseDePosteUtc : fin;
    }
}
