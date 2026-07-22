using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §8.3 "Gestion des pannes RTG".
public class RtgPanne : BaseAuditableEntity
{
    public string Engin { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public string? Raison { get; set; }
    public bool RetireEffectif { get; set; }
    public string? CommentaireReprise { get; set; }

    public TimeSpan? Duree => DateFinUtc.HasValue ? DateFinUtc.Value - DateDebutUtc : null;
    public bool EstResolue => DateFinUtc.HasValue;

    public void Cloturer(string? commentaireReprise)
    {
        DateFinUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(commentaireReprise))
        {
            CommentaireReprise = commentaireReprise;
        }
    }
}
