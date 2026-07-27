using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

public enum RopnStatus { EnCours = 0, Resolu = 1 }

// CDC §6.5 "Suivi du ROPN".
public class RopnEntry : BaseAuditableEntity
{
    public string Nom { get; set; } = string.Empty;
    public string? Role { get; set; }
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public string DifficulteRencontree { get; set; } = string.Empty;
    public string? ActionRealisee { get; set; }
    public RopnStatus Statut { get; set; } = RopnStatus.EnCours;
    public string? Commentaire { get; set; }
    public TimeSpan? Duree => DateFinUtc.HasValue ? DateFinUtc.Value - DateDebutUtc : null;

    public void Resoudre(string? actionRealisee, DateTime? dateFinUtc = null)
    {
        Statut = RopnStatus.Resolu;
        var fin = dateFinUtc ?? DateTime.UtcNow;
        DateFinUtc = fin < DateDebutUtc ? DateDebutUtc : fin;
        if (!string.IsNullOrWhiteSpace(actionRealisee))
        {
            ActionRealisee = actionRealisee;
        }
    }
}
