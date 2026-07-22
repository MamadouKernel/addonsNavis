using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §9.3 "Déconnexions ou absences" — indisponibilité de l'opérateur, distincte de celle de
// l'engin (suivie séparément dans EnginProbleme).
public class EnginDeconnexion : BaseAuditableEntity
{
    public string Engin { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateRetourUtc { get; set; }
    public string? Motif { get; set; }

    public TimeSpan? Duree => DateRetourUtc.HasValue ? DateRetourUtc.Value - DateDebutUtc : null;
    public bool EstResolu => DateRetourUtc.HasValue;

    public void SignalerRetour()
    {
        DateRetourUtc = DateTime.UtcNow;
    }
}
