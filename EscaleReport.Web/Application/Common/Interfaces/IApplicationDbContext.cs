using EscaleReport.Web.Domain.Cargo;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.VesselPlanning;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Common.Interfaces;

// Abstraction du DbContext : l'Application ne connaît ni EF Core ni le provider (SqlServer/Postgres),
// seule Infrastructure implémente cette interface (ApplicationDbContext + ses variantes par provider).
public interface IApplicationDbContext
{
    DbSet<Escale> Escales { get; }
    DbSet<UserPermission> UserPermissions { get; }
    DbSet<ContainerAnomaly> ContainerAnomalies { get; }
    DbSet<EmptyContainerTarget> EmptyContainerTargets { get; }
    DbSet<OperationalIncident> OperationalIncidents { get; }
    DbSet<AdditionalContainer> AdditionalContainers { get; }
    DbSet<DangerousContainer> DangerousContainers { get; }
    DbSet<ReferenceValue> ReferenceValues { get; }
    DbSet<Gantry> Gantries { get; }
    DbSet<GantryAssignment> GantryAssignments { get; }
    DbSet<StsIncident> StsIncidents { get; }
    DbSet<StsPointeur> StsPointeurs { get; }
    DbSet<RopnEntry> RopnEntries { get; }
    DbSet<TtEffectif> TtEffectifs { get; }
    DbSet<TtVesselAssignment> TtVesselAssignments { get; }
    DbSet<RtgEffectif> RtgEffectifs { get; }
    DbSet<RtgPanne> RtgPannes { get; }
    DbSet<RtgClash> RtgClashes { get; }
    DbSet<GateTruckIssue> GateTruckIssues { get; }
    DbSet<CargoConsommation> CargoConsommations { get; }
    DbSet<ReportEmailLog> ReportEmailLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
