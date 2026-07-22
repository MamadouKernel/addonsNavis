using EscaleReport.Web.Domain.VesselPlanning;

namespace EscaleReport.Web.Application.VesselPlanning.Dtos;

public class OperationalIncidentDto
{
    public Guid Id { get; set; }
    public string Categorie { get; set; } = string.Empty;
    public string? Localisation { get; set; }
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public IncidentStatus Statut { get; set; }
    public IncidentGravite Gravite { get; set; }
    public string? Description { get; set; }
    public string? ActionRealisee { get; set; }
    public string? DeclarePar { get; set; }

    public static OperationalIncidentDto FromEntity(OperationalIncident i) => new()
    {
        Id = i.Id,
        Categorie = i.Categorie,
        Localisation = i.Localisation,
        DateDebutUtc = i.DateDebutUtc,
        DateFinUtc = i.DateFinUtc,
        Duree = i.Duree,
        Statut = i.Statut,
        Gravite = i.Gravite,
        Description = i.Description,
        ActionRealisee = i.ActionRealisee,
        DeclarePar = i.DeclarePar
    };
}
