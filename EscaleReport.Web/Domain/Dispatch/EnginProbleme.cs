using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §9.2 "Gestion des problèmes d'engins".
public class EnginProbleme : BaseAuditableEntity
{
    public string Engin { get; set; } = string.Empty;
    public CategorieEngin Categorie { get; set; }
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public string Probleme { get; set; } = string.Empty;
    public bool RetireEffectif { get; set; }
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
