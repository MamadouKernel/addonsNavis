using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

public enum RopnStatus { EnCours = 0, Resolu = 1 }

// CDC §6.5 "Suivi du ROPN".
public class RopnEntry : BaseAuditableEntity
{
    public string Nom { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string DifficulteRencontree { get; set; } = string.Empty;
    public string? ActionRealisee { get; set; }
    public RopnStatus Statut { get; set; } = RopnStatus.EnCours;
    public string? Commentaire { get; set; }

    public void Resoudre(string? actionRealisee)
    {
        Statut = RopnStatus.Resolu;
        if (!string.IsNullOrWhiteSpace(actionRealisee))
        {
            ActionRealisee = actionRealisee;
        }
    }
}
