using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Cargo;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Coordination;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Itt;
using EscaleReport.Web.Domain.VesselPlanning;
using EscaleReport.Web.Domain.YardPlanning;
using EscaleReport.Web.Infrastructure.Identity;
using EscaleReport.Web.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Infrastructure.Persistence;

// Classe de base partagée par les deux contextes concrets (SqlServerApplicationDbContext /
// PostgresApplicationDbContext). EF Core exige un type de DbContext distinct par provider dès
// lors qu'on veut deux historiques de migrations indépendants pour le même modèle :
// https://learn.microsoft.com/ef/core/managing-schemas/migrations/providers
// Le constructeur accepte un DbContextOptions non générique pour rester agnostique du type
// concret utilisé par les sous-classes.
public abstract class ApplicationDbContext(
    DbContextOptions options,
    AuditableEntitySaveChangesInterceptor auditInterceptor)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IApplicationDbContext
{
    public DbSet<Escale> Escales => Set<Escale>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    public DbSet<ContainerAnomaly> ContainerAnomalies => Set<ContainerAnomaly>();
    public DbSet<EmptyContainerTarget> EmptyContainerTargets => Set<EmptyContainerTarget>();
    public DbSet<OperationalIncident> OperationalIncidents => Set<OperationalIncident>();
    public DbSet<AdditionalContainer> AdditionalContainers => Set<AdditionalContainer>();
    public DbSet<DangerousContainer> DangerousContainers => Set<DangerousContainer>();
    public DbSet<ReferenceValue> ReferenceValues => Set<ReferenceValue>();
    public DbSet<Gantry> Gantries => Set<Gantry>();
    public DbSet<GantryAssignment> GantryAssignments => Set<GantryAssignment>();
    public DbSet<StsIncident> StsIncidents => Set<StsIncident>();
    public DbSet<StsPointeur> StsPointeurs => Set<StsPointeur>();
    public DbSet<RopnEntry> RopnEntries => Set<RopnEntry>();
    public DbSet<TtEffectif> TtEffectifs => Set<TtEffectif>();
    public DbSet<TtVesselAssignment> TtVesselAssignments => Set<TtVesselAssignment>();
    public DbSet<RtgEffectif> RtgEffectifs => Set<RtgEffectif>();
    public DbSet<RtgPanne> RtgPannes => Set<RtgPanne>();
    public DbSet<RtgClash> RtgClashes => Set<RtgClash>();
    public DbSet<GateTruckIssue> GateTruckIssues => Set<GateTruckIssue>();
    public DbSet<AutresEnginsEffectif> AutresEnginsEffectifs => Set<AutresEnginsEffectif>();
    public DbSet<EnginProbleme> EnginProblemes => Set<EnginProbleme>();
    public DbSet<EnginDeconnexion> EnginDeconnexions => Set<EnginDeconnexion>();
    public DbSet<RemplacementOperateur> RemplacementsOperateur => Set<RemplacementOperateur>();
    public DbSet<CargoConsommation> CargoConsommations => Set<CargoConsommation>();
    public DbSet<ReportEmailLog> ReportEmailLogs => Set<ReportEmailLog>();
    public DbSet<VesselYardPlan> VesselYardPlans => Set<VesselYardPlan>();
    public DbSet<TransfertOut> TransfertsOut => Set<TransfertOut>();
    public DbSet<HousekeepingTask> HousekeepingTasks => Set<HousekeepingTask>();
    public DbSet<CoordinatorIncident> CoordinatorIncidents => Set<CoordinatorIncident>();
    public DbSet<IttTransfer> IttTransfers => Set<IttTransfer>();
    public DbSet<IttTransferIncident> IttTransferIncidents => Set<IttTransferIncident>();
    public DbSet<IttEquipementEffectif> IttEquipementEffectifs => Set<IttEquipementEffectif>();
    public DbSet<IttEnginPanne> IttEnginPannes => Set<IttEnginPanne>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(auditInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
