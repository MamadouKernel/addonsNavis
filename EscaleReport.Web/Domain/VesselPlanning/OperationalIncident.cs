using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.VesselPlanning;

public enum IncidentGravite { Information = 0, Moyen = 1, Critique = 2 }
public enum IncidentStatus { EnCours = 0, Resolu = 1 }

// CDC §5.3 "Gestion des incidents opérationnels". Catégorie = valeur paramétrable
// (ReferenceListKeys.IncidentCategory), pas un enum — même logique que les raisons d'anomalie.
public class OperationalIncident : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }

    public string Categorie { get; set; } = string.Empty;
    public string? Localisation { get; set; }
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public IncidentStatus Statut { get; set; } = IncidentStatus.EnCours;
    public IncidentGravite Gravite { get; set; } = IncidentGravite.Information;
    public string? Description { get; set; }
    public string? ActionRealisee { get; set; }
    public string? DeclarePar { get; set; }

    // CDC : "La durée devra être calculée automatiquement à partir des heures de début et de fin."
    public TimeSpan? Duree => DateFinUtc.HasValue ? DateFinUtc.Value - DateDebutUtc : null;

    public void Resoudre(string? actionRealisee)
    {
        Statut = IncidentStatus.Resolu;
        DateFinUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(actionRealisee))
        {
            ActionRealisee = actionRealisee;
        }
    }
}
