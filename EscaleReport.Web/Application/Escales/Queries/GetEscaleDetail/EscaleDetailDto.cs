using EscaleReport.Web.Application.Cargo.Dtos;
using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Application.Dispatch.Dtos;
using EscaleReport.Web.Application.Dispatch.Queries.GetDispatchTt;
using EscaleReport.Web.Application.Escales.Dtos;
using EscaleReport.Web.Application.VesselPlanning.Dtos;
using EscaleReport.Web.Domain.VesselPlanning;

namespace EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;

public class EscaleDetailDto
{
    public IReadOnlyList<EscaleHistoryItemDto> History { get; set; } = [];
    public EscaleDto Escale { get; set; } = null!;

    public PagedResult<ContainerAnomalyDto> Anomalies { get; set; } = new();
    public IReadOnlyList<string> RaisonsDisponibles { get; set; } = [];
    public IReadOnlyList<string> BaysDisponibles { get; set; } = [];
    public int AnomaliesNonResoluesCount { get; set; }

    public PagedResult<EmptyContainerTargetDto> ConteneursVides { get; set; } = new();
    // Totaux calculés sur l'ensemble des lignes de l'escale (pas seulement la page affichée) :
    // affichés en synthèse au-dessus du tableau, ils doivent rester exacts quelle que soit la page.
    public int VidesSouhaiteTotal { get; set; }
    public int VidesAjouteTotal { get; set; }
    public int VidesPlanifieTotal { get; set; }
    public int VidesEmbarqueTotal { get; set; }
    public int VidesCoupeTotal { get; set; }
    public int VidesResteTotal { get; set; }

    public PagedResult<OperationalIncidentDto> Incidents { get; set; } = new();
    public IReadOnlyList<string> CategoriesIncidentDisponibles { get; set; } = [];
    public IReadOnlyList<IncidentGravite> GravitesIncidentDisponibles { get; set; } = [];
    public int IncidentsEnCoursCount { get; set; }

    public PagedResult<AdditionalContainerDto> ConteneursAdditionnels { get; set; } = new();
    public int AdditionnelsEnAttenteCount { get; set; }

    public PagedResult<DangerousContainerDto> ConteneursDangereux { get; set; } = new();
    public int DangereuxAlertesCount { get; set; }
    public int DangereuxNonRegularisesCount { get; set; }

    // CDC §14.2 "Rapport de fin d'escale" : consommations Cargo, ressources STS/TT utilisées
    // et incidents STS rattachés à cette escale — pour une consolidation complète du rapport.
    // Non paginés : consommés uniquement par la génération PDF/Excel, jamais affichés en tableau
    // interactif dans cette vue (le rapport imprimé a besoin de la liste complète).
    public CargoConsommationDto? Cargo { get; set; }
    public IReadOnlyList<GantryAssignmentDto> RessourcesSts { get; set; } = [];
    public IReadOnlyList<TtVesselAssignmentDto> RessourcesTt { get; set; } = [];
    public IReadOnlyList<StsIncidentDto> IncidentsSts { get; set; } = [];
}
public sealed record EscaleHistoryItemDto(DateTime DateUtc, string Action, string? UserName);
