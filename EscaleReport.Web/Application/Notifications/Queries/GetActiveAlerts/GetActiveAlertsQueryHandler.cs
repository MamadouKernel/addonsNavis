using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.VesselPlanning;
using EscaleReport.Web.Domain.YardPlanning;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Notifications.Queries.GetActiveAlerts;

// CDC §16 "Notifications et alertes" — calcule en direct les 14 situations listées au CDC,
// sans état persistant : une alerte disparaît d'elle-même dès que sa cause est résolue.
public class GetActiveAlertsQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetActiveAlertsQuery, ActiveAlertsResultDto>
{
    public async Task<ActiveAlertsResultDto> Handle(GetActiveAlertsQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var alerts = new List<ActiveAlertDto>();
        var now = DateTime.UtcNow;

        var escales = await dbContext.Escales.AsNoTracking().ToListAsync(cancellationToken);
        var escalesById = escales.ToDictionary(e => e.Id);

        // 1. Escale incomplète.
        foreach (var e in escales.Where(e => e.IsDraft))
        {
            alerts.Add(Alert("Escale incomplète", $"{e.Navire} : champs obligatoires manquants (brouillon).", e));
        }

        // 2. Escale arrivée sans ATA renseignée.
        foreach (var e in escales.Where(e => e.StatutOperations == StatutOperations.EnCours && e.Ata is null))
        {
            alerts.Add(Alert("ATA manquant", $"{e.Navire} : opérations en cours sans ATA renseignée.", e));
        }

        // 3. Opération terminée sans rapport final.
        var escaleIdsAvecRapportEnvoye = await dbContext.ReportEmailLogs.AsNoTracking()
            .Where(r => r.ReportType == "RapportEscale")
            .Select(r => r.EscaleId)
            .Distinct()
            .ToListAsync(cancellationToken);
        foreach (var e in escales.Where(e => e.StatutOperations == StatutOperations.Terminees
            && !escaleIdsAvecRapportEnvoye.Contains(e.Id)))
        {
            alerts.Add(Alert("Rapport final manquant", $"{e.Navire} : opérations terminées sans rapport final envoyé.", e, AlertSeverite.Critique));
        }

        // 4. Incident critique.
        var incidentsCritiques = await dbContext.OperationalIncidents.AsNoTracking()
            .Where(i => i.Statut == IncidentStatus.EnCours && i.Gravite == IncidentGravite.Critique)
            .ToListAsync(cancellationToken);
        foreach (var i in incidentsCritiques)
        {
            escalesById.TryGetValue(i.EscaleId, out var e);
            alerts.Add(Alert("Incident critique", $"{e?.Navire ?? "Escale"} : {i.Categorie} — {i.Description}", e, AlertSeverite.Critique));
        }

        // 5. Panne d'un équipement (RTG, autres engins, ITT, portiques via camions Gate).
        var rtgPannesOuvertes = await dbContext.RtgPannes.AsNoTracking().CountAsync(p => p.DateFinUtc == null, cancellationToken);
        if (rtgPannesOuvertes > 0) { alerts.Add(new ActiveAlertDto { Type = "Panne équipement", Message = $"{rtgPannesOuvertes} panne(s) RTG en cours.", Severite = AlertSeverite.Avertissement }); }
        var enginsPannesOuvertes = await dbContext.EnginProblemes.AsNoTracking().CountAsync(p => p.DateFinUtc == null, cancellationToken);
        if (enginsPannesOuvertes > 0) { alerts.Add(new ActiveAlertDto { Type = "Panne équipement", Message = $"{enginsPannesOuvertes} panne(s) d'autres engins en cours.", Severite = AlertSeverite.Avertissement }); }
        var ittPannesOuvertes = await dbContext.IttEnginPannes.AsNoTracking().CountAsync(p => p.DateFinUtc == null, cancellationToken);
        if (ittPannesOuvertes > 0) { alerts.Add(new ActiveAlertDto { Type = "Panne équipement", Message = $"{ittPannesOuvertes} panne(s) d'engin de transfert ITT en cours.", Severite = AlertSeverite.Avertissement }); }

        // 6. Conteneur en anomalie non résolu.
        var anomaliesNonResolues = await dbContext.ContainerAnomalies.AsNoTracking()
            .Where(a => a.Statut == AnomalyStatus.NonResolu)
            .GroupBy(a => a.EscaleId)
            .Select(g => new { EscaleId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);
        foreach (var g in anomaliesNonResolues)
        {
            escalesById.TryGetValue(g.EscaleId, out var e);
            alerts.Add(Alert("Anomalie non résolue", $"{e?.Navire ?? "Escale"} : {g.Count} conteneur(s) en anomalie non résolu(s).", e));
        }

        // 7. Conteneur additionnel sans décision.
        var additionnelsEnAttente = await dbContext.AdditionalContainers.AsNoTracking()
            .Where(a => a.Decision == AdditionalContainerDecision.EnAttente)
            .GroupBy(a => a.EscaleId)
            .Select(g => new { EscaleId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);
        foreach (var g in additionnelsEnAttente)
        {
            escalesById.TryGetValue(g.EscaleId, out var e);
            alerts.Add(Alert("Additionnel sans décision", $"{e?.Navire ?? "Escale"} : {g.Count} conteneur(s) additionnel(s) sans décision.", e));
        }

        // 8. BADT à renouveler.
        var badtARenouveler = await dbContext.DangerousContainers.AsNoTracking()
            .Where(d => d.StatutBadt == BadtStatus.ARenouveler)
            .GroupBy(d => d.EscaleId)
            .Select(g => new { EscaleId = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);
        foreach (var g in badtARenouveler)
        {
            escalesById.TryGetValue(g.EscaleId, out var e);
            alerts.Add(Alert("BADT à renouveler", $"{e?.Navire ?? "Escale"} : {g.Count} BADT à renouveler.", e, AlertSeverite.Critique));
        }

        // 9/10/11. Consommations Cargo et Revised Load.
        var cargoConsommations = await dbContext.CargoConsommations.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var c in cargoConsommations)
        {
            escalesById.TryGetValue(c.EscaleId, out var e);
            if (e is null || e.StatutOperations == StatutOperations.PasEncoreDebutees) { continue; }

            if (!c.DischRealisee) { alerts.Add(Alert("Consommation Disch manquante", $"{e.Navire} : consommation Disch non renseignée.", e)); }
            if (!c.LoadRealisee) { alerts.Add(Alert("Consommation Load manquante", $"{e.Navire} : consommation Load non renseignée.", e)); }
            if (!c.RevisedLoadRecu && e.StatutOperations == StatutOperations.Terminees)
            {
                alerts.Add(Alert("Revised Load non reçu", $"{e.Navire} : Revised Load non reçu.", e));
            }
        }

        // 12. Transfert ITT en retard (heuristique : transfert non finalisé créé depuis plus de 24 h).
        var transfertsItt = await dbContext.IttTransfers.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var t in transfertsItt.Where(t => t.NombreRestant > 0 && now - t.CreatedAtUtc > TimeSpan.FromHours(24)))
        {
            alerts.Add(new ActiveAlertDto { Type = "Transfert ITT en retard", Message = $"{t.NavireConnexion} : {t.NombreRestant} conteneur(s) restant(s) depuis plus de 24 h.", Severite = AlertSeverite.Avertissement });
        }

        // 13. Tâche Yard non terminée (en retard sur sa date prévue).
        var tachesYardEnRetard = await dbContext.HousekeepingTasks.AsNoTracking()
            .CountAsync(h => h.Statut != HousekeepingStatus.Termine && h.DatePrevue != null && h.DatePrevue < now, cancellationToken);
        if (tachesYardEnRetard > 0)
        {
            alerts.Add(new ActiveAlertDto { Type = "Tâche Yard en retard", Message = $"{tachesYardEnRetard} tâche(s) Yard non terminée(s) au-delà de leur date prévue.", Severite = AlertSeverite.Avertissement });
        }

        // 14. Données d'un poste non renseignées avant la fin du shift (aucune note de relève pour le shift en cours).
        var today = DateOnly.FromDateTime(now);
        var noteAujourdhui = await dbContext.ShiftHandoverNotes.AsNoTracking().AnyAsync(n => n.Date == today, cancellationToken);
        if (!noteAujourdhui)
        {
            alerts.Add(new ActiveAlertDto { Type = "Relève de shift manquante", Message = "Aucune donnée de relève renseignée pour le shift du jour.", Severite = AlertSeverite.Info });
        }

        return new ActiveAlertsResultDto
        {
            Alertes = alerts.OrderByDescending(a => a.Severite).ToList(),
            NombreCritiques = alerts.Count(a => a.Severite == AlertSeverite.Critique)
        };
    }

    private static ActiveAlertDto Alert(string type, string message, Escale? escale, AlertSeverite severite = AlertSeverite.Avertissement) => new()
    {
        Type = type,
        Message = message,
        Severite = severite,
        EscaleId = escale?.Id,
        Navire = escale?.Navire
    };
}
