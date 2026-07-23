using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Application.Dispatch.Dtos;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchSts;

public class DispatchStsDto
{
    public IReadOnlyList<GantryDto> Gantries { get; set; } = [];
    public PagedResult<GantryAssignmentDto> Assignments { get; set; } = new();
    public IReadOnlyList<EscaleOptionDto> EscalesDisponibles { get; set; } = [];

    public PagedResult<StsIncidentDto> Incidents { get; set; } = new();
    public IReadOnlyList<string> TypesIncidentDisponibles { get; set; } = [];

    public PagedResult<StsPointeurDto> Pointeurs { get; set; } = new();
    public PagedResult<RopnEntryDto> RopnEntries { get; set; } = new();

    // Comptes "en cours" pour les pastilles d'onglet : calculés sur l'ensemble des enregistrements,
    // pas seulement la page affichée par Incidents/Pointeurs/RopnEntries.
    public int IncidentsEnCoursCount { get; set; }
    public int PointeursActifsCount { get; set; }
    public int RopnEnCoursCount { get; set; }

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
