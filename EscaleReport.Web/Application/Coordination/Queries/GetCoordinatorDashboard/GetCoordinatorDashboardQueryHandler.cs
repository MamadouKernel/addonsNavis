using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Settings;
using EscaleReport.Web.Domain.YardPlanning;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Coordination.Queries.GetCoordinatorDashboard;

public class GetCoordinatorDashboardQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetCoordinatorDashboardQuery, CoordinatorDashboardDto>
{
    public async Task<CoordinatorDashboardDto> Handle(GetCoordinatorDashboardQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterAutresModules))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterAutresModules);
        }

        // CDC §12 : ne calcule (ni n'affiche) que les modules choisis par l'Administrateur —
        // une clé absente de la table est visible par défaut (voir CoordinatorModuleVisibility).
        var hiddenModuleKeys = await dbContext.CoordinatorModuleVisibilities.AsNoTracking()
            .Where(m => !m.EstVisible).Select(m => m.Cle).ToListAsync(cancellationToken);
        var modulesVisibles = CoordinatorModuleKeys.Labels.Keys.Except(hiddenModuleKeys).ToHashSet();

        var escales = modulesVisibles.Contains(CoordinatorModuleKeys.Navires)
            ? await dbContext.Escales.AsNoTracking().ToListAsync(cancellationToken)
            : [];

        var anomaliesOuvertesParEscale = escales.Count == 0
            ? []
            : await dbContext.ContainerAnomalies
                .AsNoTracking()
                .Where(a => a.Statut != Domain.VesselPlanning.AnomalyStatus.Resolu)
                .GroupBy(a => a.EscaleId)
                .Select(g => new { EscaleId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.EscaleId, x => x.Count, cancellationToken);

        var incidentsOuvertsParEscale = escales.Count == 0
            ? []
            : await dbContext.OperationalIncidents
                .AsNoTracking()
                .Where(i => i.DateFinUtc == null)
                .GroupBy(i => i.EscaleId)
                .Select(g => new { EscaleId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.EscaleId, x => x.Count, cancellationToken);

        var syntheseNavires = escales.Select(e => new NavireSyntheseDto
        {
            Id = e.Id,
            Navire = e.Navire,
            Voyage = e.Voyage,
            LigneMaritime = e.LigneMaritime,
            Quai = e.Quai,
            Eta = e.Eta,
            Ata = e.Ata,
            Etc = e.Etc,
            StatutOperations = e.StatutOperations,
            StatutPlanification = e.StatutPlanification,
            PointsAttention =
                (anomaliesOuvertesParEscale.TryGetValue(e.Id, out var a) ? a : 0)
                + (incidentsOuvertsParEscale.TryGetValue(e.Id, out var i) ? i : 0)
        }).OrderBy(n => n.Eta).ToList();

        // ---------- Synthèse STS (§12 "Synthèse STS") ----------
        var syntheseSts = new StsSyntheseDto();
        if (modulesVisibles.Contains(CoordinatorModuleKeys.Sts))
        {
            var gantries = await dbContext.Gantries.AsNoTracking().ToListAsync(cancellationToken);
            var stsIncidentsRaw = await (
                from si in dbContext.StsIncidents.AsNoTracking()
                join e in dbContext.Escales.AsNoTracking() on si.EscaleId equals e.Id
                select new { si.DateDebutUtc, si.DateFinUtc, e.Navire }).ToListAsync(cancellationToken);

            syntheseSts = new StsSyntheseDto
            {
                PortiquesDisponibles = gantries.Count(g => g.Statut == GantryStatus.Disponible),
                PortiquesAffectes = gantries.Count(g => g.Statut == GantryStatus.Affecte),
                PortiquesEnPanne = gantries.Count(g => g.Statut == GantryStatus.EnPanne),
                IncidentsEnCours = stsIncidentsRaw.Count(x => !x.DateFinUtc.HasValue),
                // Cumul limité aux incidents déjà clôturés — un incident encore en cours n'a pas de
                // durée figée tant qu'il n'est pas résolu.
                TempsIndisponibiliteCumule = stsIncidentsRaw
                    .Where(x => x.DateFinUtc.HasValue)
                    .Aggregate(TimeSpan.Zero, (acc, x) => acc + (x.DateFinUtc!.Value - x.DateDebutUtc)),
                NaviresConcernes = stsIncidentsRaw.Where(x => !x.DateFinUtc.HasValue)
                    .Select(x => x.Navire).Distinct().ToList()
            };
        }

        // ---------- Synthèse TT (§12 "Synthèse TT") ----------
        var syntheseTt = new TtSyntheseDto();
        if (modulesVisibles.Contains(CoordinatorModuleKeys.Tt))
        {
            var ttEffectif = await dbContext.TtEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            var ecartsTt = await dbContext.TtVesselAssignments.AsNoTracking()
                .CountAsync(a => a.NombreAffecte != a.NombrePrevu, cancellationToken);
            var deconnexionsTtEnCours = await dbContext.TtDeconnexions.AsNoTracking()
                .CountAsync(d => d.DateRetourUtc == null, cancellationToken);

            syntheseTt = new TtSyntheseDto
            {
                EffectifTotal = ttEffectif?.TotalParc ?? 0,
                EffectifDesigne = ttEffectif?.Designes ?? 0,
                EffectifDisponible = ttEffectif?.Disponibles ?? 0,
                Deconnexions = deconnexionsTtEnCours,
                EcartsAffectation = ecartsTt
            };
        }

        // ---------- Synthèse RTG et autres engins (§12 "Synthèse RTG et autres engins") ----------
        var syntheseRtgAutresEngins = new RtgAutresEnginsSyntheseDto();
        if (modulesVisibles.Contains(CoordinatorModuleKeys.RtgAutresEngins))
        {
            var rtgEffectif = await dbContext.RtgEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            var autresEnginsEffectif = await dbContext.AutresEnginsEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            var enginsRetires = await dbContext.EnginProblemes.AsNoTracking()
                .CountAsync(p => p.RetireEffectif && p.DateFinUtc == null, cancellationToken);
            var clashsOuverts = await dbContext.RtgClashes.AsNoTracking().CountAsync(c => c.DateFinUtc == null, cancellationToken);
            var problemesGateOuverts = await dbContext.GateTruckIssues.AsNoTracking().CountAsync(g => g.DateFinUtc == null, cancellationToken);
            var remplacements = await dbContext.RemplacementsOperateur.AsNoTracking().CountAsync(cancellationToken);

            syntheseRtgAutresEngins = new RtgAutresEnginsSyntheseDto
            {
                RtgDisponible = rtgEffectif?.Disponible ?? 0,
                RtgEnPanne = rtgEffectif?.EnPanne ?? 0,
                RtgRetire = rtgEffectif?.Retire ?? 0,
                EnginsDisponibles = (autresEnginsEffectif?.DisponibleReachStackers ?? 0)
                    + (autresEnginsEffectif?.DisponibleEmptyHandlers ?? 0)
                    + (autresEnginsEffectif?.DisponibleAutres ?? 0),
                EnginsRetires = enginsRetires,
                Clashs = clashsOuverts,
                ProblemesGate = problemesGateOuverts,
                RemplacementsOperateurs = remplacements
            };
        }

        // ---------- Synthèse Cargo (§12 "Synthèse Cargo") ----------
        IReadOnlyList<CargoSyntheseDto> syntheseCargo = [];
        if (modulesVisibles.Contains(CoordinatorModuleKeys.Cargo))
        {
            var cargoRaw = await (
                from c in dbContext.CargoConsommations.AsNoTracking()
                join e in dbContext.Escales.AsNoTracking() on c.EscaleId equals e.Id
                select new { c, e.Navire, e.Id }).ToListAsync(cancellationToken);

            var escalesAvecRapportEnvoye = await dbContext.ReportEmailLogs.AsNoTracking()
                .Select(r => r.EscaleId).Distinct().ToListAsync(cancellationToken);

            syntheseCargo = cargoRaw.Select(x => new CargoSyntheseDto
            {
                Navire = x.Navire,
                DischFait = x.c.DischRealisee,
                LoadFait = x.c.LoadRealisee,
                RevisedLoadRecu = x.c.RevisedLoadRecu,
                RapportEnvoye = escalesAvecRapportEnvoye.Contains(x.Id),
                AlertesOuvertes = (x.c.AlerteDischNonRenseigne ? 1 : 0)
                    + (x.c.AlerteLoadNonRenseigne ? 1 : 0)
                    + (x.c.AlerteRevisedNonRecu ? 1 : 0)
            }).ToList();
        }

        // ---------- Synthèse Yard (§12 "Synthèse Yard") ----------
        var syntheseYard = new YardSyntheseDto();
        if (modulesVisibles.Contains(CoordinatorModuleKeys.Yard))
        {
            var zonesDebarquement = await dbContext.VesselYardPlans.AsNoTracking().CountAsync(cancellationToken);
            var transfertsEnCours = await dbContext.TransfertsOut.AsNoTracking().CountAsync(t => t.HeureFin == null, cancellationToken);
            var housekeepingTasks = await dbContext.HousekeepingTasks.AsNoTracking().ToListAsync(cancellationToken);
            var maintenant = DateTime.UtcNow;

            syntheseYard = new YardSyntheseDto
            {
                ZonesDebarquement = zonesDebarquement,
                TransfertsOutEnCours = transfertsEnCours,
                HousekeepingTotal = housekeepingTasks.Count,
                TachesEnRetard = housekeepingTasks.Count(t =>
                    t.Statut != HousekeepingStatus.Termine && t.DatePrevue.HasValue && t.DatePrevue.Value < maintenant)
            };
        }

        // ---------- Synthèse ITT (§12 — champs non détaillés par le CDC) ----------
        var syntheseItt = new IttSyntheseDto();
        if (modulesVisibles.Contains(CoordinatorModuleKeys.Itt))
        {
            var transfertsItt = await dbContext.IttTransfers.AsNoTracking().ToListAsync(cancellationToken);
            var incidentsIttEnCours = await dbContext.IttTransferIncidents.AsNoTracking()
                .CountAsync(i => i.DateFinUtc == null, cancellationToken);
            var equipementItt = await dbContext.IttEquipementEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            syntheseItt = new IttSyntheseDto
            {
                TransfertsEnCours = transfertsItt.Count(t => t.NombreRestant > 0),
                IncidentsEnCours = incidentsIttEnCours,
                EquipementsDisponibles = equipementItt?.Disponible ?? 0,
                EquipementsEnPanne = equipementItt?.EnPanne ?? 0
            };
        }

        // ---------- Incidents du Coordinateur (§12.8) ----------
        var incidents = await dbContext.CoordinatorIncidents.AsNoTracking()
            .OrderByDescending(i => i.DateDebutUtc)
            .Select(i => new CoordinatorIncidentDto
            {
                Id = i.Id,
                Objet = i.Objet,
                DateDebutUtc = i.DateDebutUtc,
                DateFinUtc = i.DateFinUtc,
                Duree = i.Duree,
                Note = i.Note,
                ActionRealisee = i.ActionRealisee,
                EstResolu = i.EstResolu
            }).ToListAsync(cancellationToken);

        return new CoordinatorDashboardDto
        {
            ModulesVisibles = modulesVisibles,
            SyntheseNavires = syntheseNavires,
            SyntheseSts = syntheseSts,
            SyntheseTt = syntheseTt,
            SyntheseRtgAutresEngins = syntheseRtgAutresEngins,
            SyntheseCargo = syntheseCargo,
            SyntheseYard = syntheseYard,
            SyntheseItt = syntheseItt,
            Incidents = incidents
        };
    }
}
