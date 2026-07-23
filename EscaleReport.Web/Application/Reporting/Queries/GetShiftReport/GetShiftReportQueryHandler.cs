using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Coordination.Queries.GetCoordinatorDashboard;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.YardPlanning;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Reporting.Queries.GetShiftReport;

public class GetShiftReportQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetShiftReportQuery, ShiftReportDto>
{
    public async Task<ShiftReportDto> Handle(GetShiftReportQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterRapportGeneral))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterRapportGeneral);
        }

        var escales = await dbContext.Escales.AsNoTracking().ToListAsync(cancellationToken);

        // ---------- Navires présents ou attendus (même logique que Dispatch STS §6.1) ----------
        IEnumerable<Domain.Escales.Escale> naviresQuery = escales.Where(e => e.StatutOperations != StatutOperations.Terminees
            && (e.StatutOperations == StatutOperations.EnCours
                || (DateOnly.FromDateTime(e.Eta) == request.Date
                    && (string.IsNullOrEmpty(request.Shift) || string.IsNullOrEmpty(e.Shift) || e.Shift == request.Shift))));

        if (request.EscaleId.HasValue)
        {
            naviresQuery = escales.Where(e => e.Id == request.EscaleId.Value);
        }

        var naviresPresentsOuAttendus = naviresQuery
            .OrderBy(e => e.Eta)
            .Select(e => new NavireEtatDto
            {
                Id = e.Id,
                Navire = e.Navire,
                Voyage = e.Voyage,
                Quai = e.Quai,
                Eta = e.Eta,
                StatutOperations = e.StatutOperations
            }).ToList();

        // ---------- Synthèses par poste (mêmes calculs que le tableau de bord Coordinateur) ----------
        var gantries = await dbContext.Gantries.AsNoTracking().ToListAsync(cancellationToken);
        var stsIncidentsRaw = await (
            from si in dbContext.StsIncidents.AsNoTracking()
            join e in dbContext.Escales.AsNoTracking() on si.EscaleId equals e.Id
            select new { si.DateDebutUtc, si.DateFinUtc, e.Navire, si.TypeIncident, si.GantryId }).ToListAsync(cancellationToken);

        var syntheseSts = new StsSyntheseDto
        {
            PortiquesDisponibles = gantries.Count(g => g.Statut == GantryStatus.Disponible),
            PortiquesAffectes = gantries.Count(g => g.Statut == GantryStatus.Affecte),
            PortiquesEnPanne = gantries.Count(g => g.Statut == GantryStatus.EnPanne),
            IncidentsEnCours = stsIncidentsRaw.Count(x => !x.DateFinUtc.HasValue),
            TempsIndisponibiliteCumule = stsIncidentsRaw
                .Where(x => x.DateFinUtc.HasValue)
                .Aggregate(TimeSpan.Zero, (acc, x) => acc + (x.DateFinUtc!.Value - x.DateDebutUtc)),
            NaviresConcernes = stsIncidentsRaw.Where(x => !x.DateFinUtc.HasValue).Select(x => x.Navire).Distinct().ToList()
        };

        var ttEffectif = await dbContext.TtEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var ecartsTt = await dbContext.TtVesselAssignments.AsNoTracking().CountAsync(a => a.NombreAffecte != a.NombrePrevu, cancellationToken);
        var deconnexionsTt = await dbContext.TtDeconnexions.AsNoTracking().ToListAsync(cancellationToken);

        var syntheseTt = new TtSyntheseDto
        {
            EffectifTotal = ttEffectif?.TotalParc ?? 0,
            EffectifDesigne = ttEffectif?.Designes ?? 0,
            EffectifDisponible = ttEffectif?.Disponibles ?? 0,
            Deconnexions = deconnexionsTt.Count(d => !d.EstResolue),
            EcartsAffectation = ecartsTt
        };

        var rtgEffectif = await dbContext.RtgEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var autresEnginsEffectif = await dbContext.AutresEnginsEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var rtgPannes = await dbContext.RtgPannes.AsNoTracking().ToListAsync(cancellationToken);
        var enginProblemes = await dbContext.EnginProblemes.AsNoTracking().ToListAsync(cancellationToken);
        var rtgClashes = await dbContext.RtgClashes.AsNoTracking().ToListAsync(cancellationToken);
        var gateIssues = await dbContext.GateTruckIssues.AsNoTracking().ToListAsync(cancellationToken);
        var remplacements = await dbContext.RemplacementsOperateur.AsNoTracking().CountAsync(cancellationToken);

        var syntheseRtgAutresEngins = new RtgAutresEnginsSyntheseDto
        {
            RtgDisponible = rtgEffectif?.Disponible ?? 0,
            RtgEnPanne = rtgEffectif?.EnPanne ?? 0,
            RtgRetire = rtgEffectif?.Retire ?? 0,
            EnginsDisponibles = (autresEnginsEffectif?.DisponibleReachStackers ?? 0)
                + (autresEnginsEffectif?.DisponibleEmptyHandlers ?? 0)
                + (autresEnginsEffectif?.DisponibleAutres ?? 0),
            EnginsRetires = enginProblemes.Count(p => p.RetireEffectif && !p.EstResolu),
            Clashs = rtgClashes.Count(c => !c.EstResolu),
            ProblemesGate = gateIssues.Count(g => !g.EstResolu),
            RemplacementsOperateurs = remplacements
        };

        var cargoRaw = await (
            from c in dbContext.CargoConsommations.AsNoTracking()
            join e in dbContext.Escales.AsNoTracking() on c.EscaleId equals e.Id
            select new { c, e.Navire, e.Id }).ToListAsync(cancellationToken);
        var escalesAvecRapportEnvoye = await dbContext.ReportEmailLogs.AsNoTracking().Select(r => r.EscaleId).Distinct().ToListAsync(cancellationToken);

        var syntheseCargo = cargoRaw.Select(x => new CargoSyntheseDto
        {
            Navire = x.Navire,
            DischFait = x.c.DischRealisee,
            LoadFait = x.c.LoadRealisee,
            RevisedLoadRecu = x.c.RevisedLoadRecu,
            RapportEnvoye = escalesAvecRapportEnvoye.Contains(x.Id),
            AlertesOuvertes = (x.c.AlerteDischNonRenseigne ? 1 : 0) + (x.c.AlerteLoadNonRenseigne ? 1 : 0) + (x.c.AlerteRevisedNonRecu ? 1 : 0)
        }).ToList();

        var zonesDebarquement = await dbContext.VesselYardPlans.AsNoTracking().CountAsync(cancellationToken);
        var transfertsOutEnCours = await dbContext.TransfertsOut.AsNoTracking().CountAsync(t => t.HeureFin == null, cancellationToken);
        var housekeepingTasks = await dbContext.HousekeepingTasks.AsNoTracking().ToListAsync(cancellationToken);
        var maintenant = DateTime.UtcNow;

        var syntheseYard = new YardSyntheseDto
        {
            ZonesDebarquement = zonesDebarquement,
            TransfertsOutEnCours = transfertsOutEnCours,
            HousekeepingTotal = housekeepingTasks.Count,
            TachesEnRetard = housekeepingTasks.Count(t => t.Statut != HousekeepingStatus.Termine && t.DatePrevue.HasValue && t.DatePrevue.Value < maintenant)
        };

        var transfertsItt = await dbContext.IttTransfers.AsNoTracking().ToListAsync(cancellationToken);
        var incidentsItt = await dbContext.IttTransferIncidents.AsNoTracking().ToListAsync(cancellationToken);
        var equipementItt = await dbContext.IttEquipementEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var pannesItt = await dbContext.IttEnginPannes.AsNoTracking().ToListAsync(cancellationToken);

        var syntheseItt = new IttSyntheseDto
        {
            TransfertsEnCours = transfertsItt.Count(t => t.NombreRestant > 0),
            IncidentsEnCours = incidentsItt.Count(i => !i.EstResolu),
            EquipementsDisponibles = equipementItt?.Disponible ?? 0,
            EquipementsEnPanne = equipementItt?.EnPanne ?? 0
        };

        // ---------- Pannes et indisponibilités / Incidents (vue transverse, §14.1) ----------
        var gantryCodes = gantries.ToDictionary(g => g.Id, g => g.Code);
        var pannesEtIndisponibilites = new List<string>();
        pannesEtIndisponibilites.AddRange(rtgPannes.Where(p => !p.EstResolue).Select(p => $"RTG {p.Engin} : {p.Raison ?? "panne en cours"}"));
        pannesEtIndisponibilites.AddRange(enginProblemes.Where(p => !p.EstResolu).Select(p => $"{p.Engin} ({p.Categorie}) : {p.Probleme}"));
        pannesEtIndisponibilites.AddRange(pannesItt.Where(p => !p.EstResolue).Select(p => $"ITT {p.Engin} : {p.Cause ?? "panne en cours"}"));
        pannesEtIndisponibilites.AddRange(gateIssues.Where(g => !g.EstResolu).Select(g => $"Camion Gate {g.CamionReference} : {g.ProblemeRencontre}"));
        pannesEtIndisponibilites.AddRange(rtgClashes.Where(c => !c.EstResolu).Select(c => $"Clash {c.Lieu} : {c.Description}"));

        var incidents = new List<string>();
        incidents.AddRange(stsIncidentsRaw.Where(x => !x.DateFinUtc.HasValue)
            .Select(x => $"STS {(x.GantryId.HasValue && gantryCodes.TryGetValue(x.GantryId.Value, out var code) ? code : "—")} ({x.Navire}) : {x.TypeIncident}"));
        var incidentsOperationnels = await (
            from i in dbContext.OperationalIncidents.AsNoTracking()
            join e in dbContext.Escales.AsNoTracking() on i.EscaleId equals e.Id
            where i.DateFinUtc == null
            select new { i.Categorie, i.Description, e.Navire }).ToListAsync(cancellationToken);
        incidents.AddRange(incidentsOperationnels.Select(x => $"{x.Categorie} ({x.Navire}) : {x.Description ?? "—"}"));
        incidents.AddRange(incidentsItt.Where(i => !i.EstResolu).Select(i => $"ITT : {i.DifficulteOuObjet}"));

        // ---------- Actions en cours / Points à transmettre (saisie du Coordinateur) ----------
        var handover = await dbContext.ShiftHandoverNotes.AsNoTracking()
            .FirstOrDefaultAsync(n => n.Date == request.Date && n.Shift == request.Shift, cancellationToken);

        // ---------- Planification (§14.1 : navires attendus + consigne par navire) ----------
        var planificationNotes = await dbContext.EscalePlanificationNotes.AsNoTracking()
            .ToDictionaryAsync(n => n.EscaleId, n => n.Commentaire, cancellationToken);

        var planification = escales
            .Where(e => e.StatutOperations != StatutOperations.Terminees)
            .OrderBy(e => e.Eta)
            .Select(e => new PlanificationNavireDto
            {
                EscaleId = e.Id,
                Navire = e.Navire,
                Voyage = e.Voyage,
                Quai = e.Quai,
                Eta = e.Eta,
                StatutPlanification = e.StatutPlanification,
                Commentaire = planificationNotes.TryGetValue(e.Id, out var c) ? c : null
            }).ToList();

        var escalesDisponibles = escales
            .Where(e => e.StatutOperations != StatutOperations.Terminees)
            .OrderBy(e => e.Navire)
            .Select(e => new EscaleOptionDto { Id = e.Id, Navire = e.Navire })
            .ToList();

        var shiftsDisponibles = await dbContext.ReferenceValues
            .AsNoTracking()
            .Where(r => r.ListKey == Domain.Common.ReferenceListKeys.Shift && r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);

        return new ShiftReportDto
        {
            Date = request.Date,
            Shift = request.Shift,
            EscalesDisponibles = escalesDisponibles,
            ShiftsDisponibles = shiftsDisponibles,
            Coordinateur = currentUser.UserName ?? "—",
            NavireCourantMode = request.EscaleId.HasValue,
            NaviresPresentsOuAttendus = naviresPresentsOuAttendus,
            SyntheseSts = syntheseSts,
            SyntheseTt = syntheseTt,
            SyntheseRtgAutresEngins = syntheseRtgAutresEngins,
            SyntheseCargo = syntheseCargo,
            SyntheseYard = syntheseYard,
            SyntheseItt = syntheseItt,
            PannesEtIndisponibilites = pannesEtIndisponibilites,
            Incidents = incidents,
            ActionsEnCours = handover?.ActionsEnCours,
            PointsATransmettre = handover?.PointsATransmettre,
            ValidePar = handover?.ValidePar,
            ValideLeUtc = handover?.ValideLeUtc,
            CommentaireValidation = handover?.CommentaireValidation,
            PriseDeConnaissanceParUtilisateur = handover?.PriseDeConnaissanceParUtilisateur,
            PriseDeConnaissanceLeUtc = handover?.PriseDeConnaissanceLeUtc,
            Planification = planification
        };
    }
}
