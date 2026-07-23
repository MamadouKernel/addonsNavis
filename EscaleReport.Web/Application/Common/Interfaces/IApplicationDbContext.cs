using EscaleReport.Web.Domain.Audit;
using EscaleReport.Web.Domain.Cargo;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Coordination;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Itt;
using EscaleReport.Web.Domain.Reporting;
using EscaleReport.Web.Domain.Settings;
using EscaleReport.Web.Domain.VesselPlanning;
using EscaleReport.Web.Domain.YardPlanning;
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
    DbSet<TtDeconnexion> TtDeconnexions { get; }
    DbSet<RtgEffectif> RtgEffectifs { get; }
    DbSet<RtgPanne> RtgPannes { get; }
    DbSet<RtgClash> RtgClashes { get; }
    DbSet<GateTruckIssue> GateTruckIssues { get; }
    DbSet<AutresEnginsEffectif> AutresEnginsEffectifs { get; }
    DbSet<EnginProbleme> EnginProblemes { get; }
    DbSet<EnginDeconnexion> EnginDeconnexions { get; }
    DbSet<RemplacementOperateur> RemplacementsOperateur { get; }
    DbSet<CargoConsommation> CargoConsommations { get; }
    DbSet<ReportEmailLog> ReportEmailLogs { get; }
    DbSet<VesselYardPlan> VesselYardPlans { get; }
    DbSet<TransfertOut> TransfertsOut { get; }
    DbSet<HousekeepingTask> HousekeepingTasks { get; }
    DbSet<CoordinatorIncident> CoordinatorIncidents { get; }
    DbSet<IttTransfer> IttTransfers { get; }
    DbSet<IttTransferIncident> IttTransferIncidents { get; }
    DbSet<IttEquipementEffectif> IttEquipementEffectifs { get; }
    DbSet<IttEnginPanne> IttEnginPannes { get; }
    DbSet<ShiftHandoverNote> ShiftHandoverNotes { get; }
    DbSet<EscalePlanificationNote> EscalePlanificationNotes { get; }
    DbSet<GeneralSettings> GeneralSettings { get; }
    DbSet<EmailTemplate> EmailTemplates { get; }
    DbSet<AlertThreshold> AlertThresholds { get; }
    DbSet<AuditLogEntry> AuditLogEntries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
