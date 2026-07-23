using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Application.Dispatch.Dtos;
using EscaleReport.Web.Domain.Dispatch;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchRtg;

public class DispatchRtgDto
{
    public RtgEffectifDto Effectif { get; set; } = new();
    public PagedResult<RtgPanneDto> Pannes { get; set; } = new();
    public PagedResult<RtgClashDto> Clashes { get; set; } = new();
    public PagedResult<GateTruckIssueDto> GateTruckIssues { get; set; } = new();

    // CDC §8.2 : incidents STS du même shift, en lecture seule (contexte navire pour le
    // dispatcher RTG, sans lui donner la main pour les modifier — ce périmètre appartient à STS).
    public PagedResult<StsIncidentDto> StsIncidents { get; set; } = new();

    // Comptes "en cours" pour les pastilles d'onglet : calculés sur l'ensemble des enregistrements.
    public int PannesEnCoursCount { get; set; }
    public int ClashsEnCoursCount { get; set; }
    public int GateEnCoursCount { get; set; }
    public int IncidentsStsEnCoursCount { get; set; }
}

public class RtgEffectifDto
{
    public int TotalParc { get; set; }
    public int Disponible { get; set; }
    public int Affecte { get; set; }
    public int EnPanne { get; set; }
    public int Retire { get; set; }
}

public class RtgPanneDto
{
    public Guid Id { get; set; }
    public string Engin { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string? Raison { get; set; }
    public bool RetireEffectif { get; set; }
    public string? CommentaireReprise { get; set; }
    public bool EstResolue { get; set; }
}

public class RtgClashDto
{
    public Guid Id { get; set; }
    public string Lieu { get; set; } = string.Empty;
    public string EnginsConcernes { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? ActionRealisee { get; set; }
    public bool EstResolu { get; set; }
}

public class GateTruckIssueDto
{
    public Guid Id { get; set; }
    public GateOperationType TypeOperation { get; set; }
    public string CamionReference { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string ProblemeRencontre { get; set; } = string.Empty;
    public string? ActionRealisee { get; set; }
    public bool EstResolu { get; set; }
}
