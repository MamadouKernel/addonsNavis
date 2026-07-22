using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.YardPlanning;

// CDC §11.2 "Plans navire et zones de débarquement". Un navire peut avoir plusieurs zones
// de débarquement, d'où une liste (et non un enregistrement unique par escale).
public class VesselYardPlan : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }
    public string ServiceMaritime { get; set; } = string.Empty;
    public string ZoneDebarquement { get; set; } = string.Empty;
    public int ReefersImport { get; set; }
    public int ReefersExport { get; set; }
    public int ConteneursTransbordement { get; set; }
    public string? Observations { get; set; }
}
