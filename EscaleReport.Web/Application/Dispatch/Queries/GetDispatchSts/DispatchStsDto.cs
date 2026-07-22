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

    // CDC §6.1 "Sélection du shift".
    public DateOnly SelectedDate { get; set; }
    public string? SelectedShift { get; set; }
    public IReadOnlyList<string> ShiftsDisponibles { get; set; } = [];
    public IReadOnlyList<NavireDuShiftDto> NaviresDuShift { get; set; } = [];
}

public class NavireDuShiftDto
{
    public Guid Id { get; set; }
    public string Navire { get; set; } = string.Empty;
    public string Voyage { get; set; } = string.Empty;
    public DateTime Eta { get; set; }
    public string? Shift { get; set; }
    public bool EnCours { get; set; }
}
