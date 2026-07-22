using EscaleReport.Web.Application.Cargo.Dtos;
using EscaleReport.Web.Application.Dispatch.Dtos;
using EscaleReport.Web.Application.Dispatch.Queries.GetDispatchTt;
using EscaleReport.Web.Application.Escales.Dtos;
using EscaleReport.Web.Application.VesselPlanning.Dtos;

namespace EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;

public class EscaleDetailDto
{
    public EscaleDto Escale { get; set; } = null!;

    public IReadOnlyList<ContainerAnomalyDto> Anomalies { get; set; } = [];
    public IReadOnlyList<string> RaisonsDisponibles { get; set; } = [];

    public IReadOnlyList<EmptyContainerTargetDto> ConteneursVides { get; set; } = [];

    public IReadOnlyList<OperationalIncidentDto> Incidents { get; set; } = [];
    public IReadOnlyList<string> CategoriesIncidentDisponibles { get; set; } = [];

    public IReadOnlyList<AdditionalContainerDto> ConteneursAdditionnels { get; set; } = [];

    public IReadOnlyList<DangerousContainerDto> ConteneursDangereux { get; set; } = [];

    // CDC §14.2 "Rapport de fin d'escale" : consommations Cargo, ressources STS/TT utilisées
    // et incidents STS rattachés à cette escale — pour une consolidation complète du rapport.
    public CargoConsommationDto? Cargo { get; set; }
    public IReadOnlyList<GantryAssignmentDto> RessourcesSts { get; set; } = [];
    public IReadOnlyList<TtVesselAssignmentDto> RessourcesTt { get; set; } = [];
    public IReadOnlyList<StsIncidentDto> IncidentsSts { get; set; } = [];
}
