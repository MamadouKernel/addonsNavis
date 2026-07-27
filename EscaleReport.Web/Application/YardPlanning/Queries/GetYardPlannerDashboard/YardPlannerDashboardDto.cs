using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.YardPlanning;

namespace EscaleReport.Web.Application.YardPlanning.Queries.GetYardPlannerDashboard;

public class YardPlannerDashboardDto
{
    public PagedResult<NavireEnCoursDto> NaviresEnCours { get; set; } = new();
    public PagedResult<VesselYardPlanDto> VesselYardPlans { get; set; } = new();
    public PagedResult<TransfertOutDto> TransfertsOut { get; set; } = new();
    public PagedResult<HousekeepingTaskDto> HousekeepingTasks { get; set; } = new();
    public IReadOnlyList<EscaleYardOptionDto> EscalesDisponibles { get; set; } = [];
    public IReadOnlyList<string> ZonesDisponibles { get; set; } = [];
    public IReadOnlyList<string> BaysDisponibles { get; set; } = [];

    // Comptes pour les pastilles d'onglet, calculés sur l'ensemble des enregistrements.
    public int TransfertsEnCoursCount { get; set; }
    public int TachesEnCoursCount { get; set; }
}

// CDC §11.1 "Navires en cours à quai".
public class NavireEnCoursDto
{
    public Guid Id { get; set; }
    public string Navire { get; set; } = string.Empty;
    public string Voyage { get; set; } = string.Empty;
    public string? Quai { get; set; }
    public string LigneMaritime { get; set; } = string.Empty;
    public DateTime Eta { get; set; }
    public StatutOperations StatutOperations { get; set; }
    public StatutPlanification StatutPlanification { get; set; }
}

public class EscaleYardOptionDto
{
    public Guid Id { get; set; }
    public string Navire { get; set; } = string.Empty;
}

public class VesselYardPlanDto
{
    public Guid Id { get; set; }
    public string Navire { get; set; } = string.Empty;
    public string ServiceMaritime { get; set; } = string.Empty;
    public string ZoneDebarquement { get; set; } = string.Empty;
    public int ReefersImport { get; set; }
    public int ReefersExport { get; set; }
    public int ConteneursTransbordement { get; set; }
    public string? Observations { get; set; }
}

public class TransfertOutDto
{
    public Guid Id { get; set; }
    public string Bay { get; set; } = string.Empty;
    public int NombreConteneurs { get; set; }
    public string? DetailOuDestination { get; set; }
    public DateTime HeureDebut { get; set; }
    public DateTime? HeureFin { get; set; }
    public string? Commentaire { get; set; }
    public bool EstTermine { get; set; }
}

public class HousekeepingTaskDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Zone { get; set; }
    public HousekeepingStatus Statut { get; set; }
    public string? Priorite { get; set; }
    public string? Responsable { get; set; }
    public DateTime? DatePrevue { get; set; }
    public DateTime? DateRealisation { get; set; }
    public string? Note { get; set; }
}
