using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.VesselPlanning;
using EscaleReport.Web.Domain.YardPlanning;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Statistics.Queries.GetIndicators;

// CDC §17 "Reporting et indicateurs" — les indicateurs rattachés à une escale (anomalies,
// incidents, conteneurs...) respectent les filtres période/shift/navire/ligne/quai/type
// d'incident. Les effectifs STS/TT/RTG/autres engins suivent le principe "courant" déjà en
// place pour ces modules (pas d'historique par shift) : leurs ratios de disponibilité restent
// donc un instantané global, non filtrable par période — cohérent avec §6-9.
// Filtres équipement/utilisateur/équipe (extension) : "équipement" matche le code de portique
// (Gantry.Code) ou le champ libre "Engin" des pannes RTG/autres engins/ITT ; "utilisateur"
// matche BaseAuditableEntity.CreatedBy, déjà renseigné automatiquement par
// AuditableEntitySaveChangesInterceptor sur chaque enregistrement, sans aucune migration ;
// "équipe" résout CreatedBy vers ApplicationUser.Equipe via IUserDirectoryService.
public class GetIndicatorsQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser,
    IUserDirectoryService userDirectory) : IRequestHandler<GetIndicatorsQuery, IndicatorsResultDto>
{
    public async Task<IndicatorsResultDto> Handle(GetIndicatorsQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterStatistiques))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterStatistiques);
        }

        var equipesByUser = await userDirectory.GetEquipesByUserNameAsync(cancellationToken);

        IEnumerable<T> ParUtilisateurEtEquipe<T>(IEnumerable<T> items) where T : BaseAuditableEntity
        {
            var filtered = items;
            if (!string.IsNullOrWhiteSpace(request.Utilisateur))
            {
                filtered = filtered.Where(i => i.CreatedBy == request.Utilisateur);
            }

            if (!string.IsNullOrWhiteSpace(request.Equipe))
            {
                filtered = filtered.Where(i =>
                    i.CreatedBy is not null &&
                    equipesByUser.TryGetValue(i.CreatedBy, out var equipe) &&
                    equipe == request.Equipe);
            }

            return filtered;
        }

        var escalesQuery = dbContext.Escales.AsNoTracking().AsQueryable();
        if (request.DateDebut is { } debut) { var d = debut.ToDateTime(TimeOnly.MinValue); escalesQuery = escalesQuery.Where(e => e.Eta >= d); }
        if (request.DateFin is { } fin) { var f = fin.ToDateTime(TimeOnly.MaxValue); escalesQuery = escalesQuery.Where(e => e.Eta <= f); }
        if (!string.IsNullOrWhiteSpace(request.Shift)) { escalesQuery = escalesQuery.Where(e => e.Shift == request.Shift); }
        if (!string.IsNullOrWhiteSpace(request.Navire)) { escalesQuery = escalesQuery.Where(e => e.Navire == request.Navire); }
        if (!string.IsNullOrWhiteSpace(request.LigneMaritime)) { escalesQuery = escalesQuery.Where(e => e.LigneMaritime == request.LigneMaritime); }
        if (!string.IsNullOrWhiteSpace(request.Quai)) { escalesQuery = escalesQuery.Where(e => e.Quai == request.Quai); }

        var escales = await escalesQuery.ToListAsync(cancellationToken);
        var escaleIds = escales.Select(e => e.Id).ToHashSet();

        var anomalies = ParUtilisateurEtEquipe(await dbContext.ContainerAnomalies.AsNoTracking()
            .Where(a => escaleIds.Contains(a.EscaleId)).ToListAsync(cancellationToken)).ToList();

        var incidentsQuery = dbContext.OperationalIncidents.AsNoTracking().Where(i => escaleIds.Contains(i.EscaleId));
        if (!string.IsNullOrWhiteSpace(request.TypeIncident)) { incidentsQuery = incidentsQuery.Where(i => i.Categorie == request.TypeIncident); }
        var incidents = ParUtilisateurEtEquipe(await incidentsQuery.ToListAsync(cancellationToken)).ToList();

        var additionnels = ParUtilisateurEtEquipe(await dbContext.AdditionalContainers.AsNoTracking()
            .Where(a => escaleIds.Contains(a.EscaleId)).ToListAsync(cancellationToken)).Count();
        var dangereux = ParUtilisateurEtEquipe(await dbContext.DangerousContainers.AsNoTracking()
            .Where(d => escaleIds.Contains(d.EscaleId)).ToListAsync(cancellationToken)).Count();
        var videsTargets = ParUtilisateurEtEquipe(await dbContext.EmptyContainerTargets.AsNoTracking()
            .Where(v => escaleIds.Contains(v.EscaleId)).ToListAsync(cancellationToken)).ToList();
        var cargoConsommations = ParUtilisateurEtEquipe(await dbContext.CargoConsommations.AsNoTracking()
            .Where(c => escaleIds.Contains(c.EscaleId)).ToListAsync(cancellationToken)).ToList();
        var reportLogs = ParUtilisateurEtEquipe(await dbContext.ReportEmailLogs.AsNoTracking()
            .Where(r => escaleIds.Contains(r.EscaleId) && r.ReportType == "RapportEscale").ToListAsync(cancellationToken)).ToList();

        // Effectifs "courants" (instantané global, cf. commentaire de classe).
        var rtg = await dbContext.RtgEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var tt = await dbContext.TtEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var autresEngins = await dbContext.AutresEnginsEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        var gantriesTous = await dbContext.Gantries.AsNoTracking().ToListAsync(cancellationToken);
        var gantries = string.IsNullOrWhiteSpace(request.Equipement)
            ? gantriesTous
            : gantriesTous.Where(g => g.Code == request.Equipement).ToList();
        var gantryIds = gantries.Select(g => g.Id).ToHashSet();
        var gantryAssignmentsOuvertes = await dbContext.GantryAssignments.AsNoTracking()
            .CountAsync(a => a.HeureFin == null && gantryIds.Contains(a.GantryId), cancellationToken);

        var rtgPannes = ParUtilisateurEtEquipe(await dbContext.RtgPannes.AsNoTracking().ToListAsync(cancellationToken))
            .Where(p => string.IsNullOrWhiteSpace(request.Equipement) || p.Engin == request.Equipement).ToList();
        var enginPannes = ParUtilisateurEtEquipe(await dbContext.EnginProblemes.AsNoTracking().ToListAsync(cancellationToken))
            .Where(p => string.IsNullOrWhiteSpace(request.Equipement) || p.Engin == request.Equipement).ToList();
        var ittPannes = ParUtilisateurEtEquipe(await dbContext.IttEnginPannes.AsNoTracking().ToListAsync(cancellationToken))
            .Where(p => string.IsNullOrWhiteSpace(request.Equipement) || p.Engin == request.Equipement).ToList();
        var nombrePannes = rtgPannes.Count + enginPannes.Count + ittPannes.Count;
        var dureeTotalePannesHeures =
            rtgPannes.Sum(p => (p.DateFinUtc ?? DateTime.UtcNow).Subtract(p.DateDebutUtc).TotalHours) +
            enginPannes.Sum(p => (p.DateFinUtc ?? DateTime.UtcNow).Subtract(p.DateDebutUtc).TotalHours) +
            ittPannes.Sum(p => (p.DateFinUtc ?? DateTime.UtcNow).Subtract(p.DateDebutUtc).TotalHours);

        var transferts = ParUtilisateurEtEquipe(await dbContext.IttTransfers.AsNoTracking().ToListAsync(cancellationToken)).ToList();
        var tachesYardRealisees = ParUtilisateurEtEquipe(await dbContext.HousekeepingTasks.AsNoTracking()
            .Where(h => h.Statut == HousekeepingStatus.Termine).ToListAsync(cancellationToken)).Count();

        var naviresDisponibles = await dbContext.Escales.AsNoTracking().Select(e => e.Navire).Distinct().OrderBy(n => n).ToListAsync(cancellationToken);
        var lignesDisponibles = await dbContext.Escales.AsNoTracking().Select(e => e.LigneMaritime).Where(l => l != "").Distinct().OrderBy(l => l).ToListAsync(cancellationToken);
        var quaisDisponibles = await dbContext.Escales.AsNoTracking().Where(e => e.Quai != null).Select(e => e.Quai!).Distinct().OrderBy(q => q).ToListAsync(cancellationToken);
        var shiftsDisponibles = await dbContext.ReferenceValues.AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.Shift && r.IsActive).OrderBy(r => r.SortOrder).Select(r => r.Value).ToListAsync(cancellationToken);
        var categoriesIncident = await dbContext.ReferenceValues.AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.IncidentCategory && r.IsActive).OrderBy(r => r.SortOrder).Select(r => r.Value).ToListAsync(cancellationToken);

        var equipementsDisponibles = gantriesTous.Select(g => g.Code)
            .Concat(await dbContext.RtgPannes.AsNoTracking().Select(p => p.Engin).ToListAsync(cancellationToken))
            .Concat(await dbContext.EnginProblemes.AsNoTracking().Select(p => p.Engin).ToListAsync(cancellationToken))
            .Concat(await dbContext.IttEnginPannes.AsNoTracking().Select(p => p.Engin).ToListAsync(cancellationToken))
            .Where(e => !string.IsNullOrWhiteSpace(e)).Distinct().OrderBy(e => e).ToList();
        var utilisateursDisponibles = equipesByUser.Keys.OrderBy(u => u).ToList();
        var equipesDisponibles = await userDirectory.GetEquipesDisponiblesAsync(cancellationToken);

        var anomaliesResolues = anomalies.Where(a => a.DateResolutionUtc is not null).ToList();

        return new IndicatorsResultDto
        {
            NaviresDisponibles = naviresDisponibles,
            LignesDisponibles = lignesDisponibles,
            QuaisDisponibles = quaisDisponibles,
            ShiftsDisponibles = shiftsDisponibles,
            CategoriesIncidentDisponibles = categoriesIncident,
            EquipementsDisponibles = equipementsDisponibles,
            UtilisateursDisponibles = utilisateursDisponibles,
            EquipesDisponibles = equipesDisponibles,

            NombreEscales = escales.Count,
            NombreEscalesTerminees = escales.Count(e => e.StatutOperations == StatutOperations.Terminees),
            NombreEscalesAvecAnomalies = anomalies.Select(a => a.EscaleId).Distinct().Count(),

            NombreConteneursEnAnomalie = anomalies.Count,
            DelaiMoyenResolutionAnomaliesHeures = anomaliesResolues.Count > 0
                ? anomaliesResolues.Average(a => (a.DateResolutionUtc!.Value - a.CreatedAtUtc).TotalHours)
                : null,

            IncidentsParCategorie = incidents.GroupBy(i => i.Categorie)
                .Select(g => (g.Key, g.Count())).OrderByDescending(x => x.Item2).ToList(),
            NombreIncidentsCritiques = incidents.Count(i => i.Gravite == IncidentGravite.Critique),
            DureeTotaleIncidentsHeures = incidents.Sum(i => (i.DateFinUtc ?? DateTime.UtcNow).Subtract(i.DateDebutUtc).TotalHours),

            DisponibiliteStsPct = gantries.Count > 0 ? Pct(gantries.Count(g => g.Statut == Domain.Dispatch.GantryStatus.Disponible), gantries.Count) : 0,
            DisponibiliteTtPct = tt is { TotalParc: > 0 } ? Pct(tt.TotalParc - tt.Retires, tt.TotalParc) : 0,
            DisponibiliteRtgPct = rtg is { TotalParc: > 0 } ? Pct(rtg.Disponible, rtg.TotalParc) : 0,
            DisponibiliteAutresEnginsPct = autresEngins is not null
                ? Pct(autresEngins.DisponibleReachStackers + autresEngins.DisponibleEmptyHandlers + autresEngins.DisponibleAutres, Math.Max(1, autresEngins.DisponibleReachStackers + autresEngins.DisponibleEmptyHandlers + autresEngins.DisponibleAutres))
                : 0,
            NombrePannes = nombrePannes,
            DureeTotalePannesHeures = dureeTotalePannesHeures,
            TauxAffectationEquipementsPct = gantries.Count > 0 ? Pct(gantryAssignmentsOuvertes, gantries.Count) : 0,

            ConteneursVidesSouhaites = videsTargets.Sum(v => v.QuantiteSouhaitee + v.QuantiteAjoutee),
            ConteneursVidesEmbarques = videsTargets.Sum(v => v.QuantiteEmbarquee),
            ConteneursVidesCoupes = videsTargets.Sum(v => v.QuantiteCoupee),
            ConteneursVidesRestants = videsTargets.Sum(v => Math.Max(0, v.QuantiteSouhaitee + v.QuantiteAjoutee - v.QuantiteEmbarquee - v.QuantiteCoupee)),
            NombreConteneursAdditionnels = additionnels,
            NombreConteneursDangereux = dangereux,

            TauxRealisationDischPct = cargoConsommations.Count > 0 ? Pct(cargoConsommations.Count(c => c.DischRealisee), cargoConsommations.Count) : 0,
            TauxRealisationLoadPct = cargoConsommations.Count > 0 ? Pct(cargoConsommations.Count(c => c.LoadRealisee), cargoConsommations.Count) : 0,

            DelaiMoyenProductionRapportsHeures = ComputeDelaiRapports(escales, reportLogs),
            NombreRapportsEnvoyes = reportLogs.Count,

            NombreTransfertsItt = transferts.Count,
            TauxAvancementTransfertsPct = transferts.Count > 0 ? transferts.Average(t => t.PourcentageAvancement) : 0,

            NombreTachesYardRealisees = tachesYardRealisees
        };
    }

    private static double Pct(int part, int total) => total <= 0 ? 0 : Math.Round(100.0 * part / total, 1);

    private static double? ComputeDelaiRapports(List<Escale> escales, List<ReportEmailLog> logs)
    {
        var delais = new List<double>();
        foreach (var log in logs)
        {
            var escale = escales.FirstOrDefault(e => e.Id == log.EscaleId);
            if (escale is not null && escale.Etc is not null)
            {
                delais.Add((log.SentAtUtc - escale.Etc.Value).TotalHours);
            }
        }

        return delais.Count > 0 ? Math.Round(delais.Average(), 1) : null;
    }
}
