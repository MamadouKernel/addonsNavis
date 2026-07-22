using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Coordination;

// CDC §12.8 "Incidents du Coordinateur" — incidents propres au poste de Coordinateur Control
// Room, distincts des incidents déclarés par les autres postes (STS, Vessel Planning...).
public class CoordinatorIncident : BaseAuditableEntity
{
    public string Objet { get; set; } = string.Empty;
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
