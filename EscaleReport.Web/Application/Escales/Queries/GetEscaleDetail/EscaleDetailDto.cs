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
}
