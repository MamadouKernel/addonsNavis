using EscaleReport.Web.Application.Dispatch.Dtos;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchSts;

public class DispatchStsDto
{
    public IReadOnlyList<GantryDto> Gantries { get; set; } = [];
    public IReadOnlyList<GantryAssignmentDto> Assignments { get; set; } = [];
    public IReadOnlyList<EscaleOptionDto> EscalesDisponibles { get; set; } = [];

    public IReadOnlyList<StsIncidentDto> Incidents { get; set; } = [];
    public IReadOnlyList<string> TypesIncidentDisponibles { get; set; } = [];

    public IReadOnlyList<StsPointeurDto> Pointeurs { get; set; } = [];
    public IReadOnlyList<RopnEntryDto> RopnEntries { get; set; } = [];
}
