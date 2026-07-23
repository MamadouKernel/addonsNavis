using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Application.Dispatch.Dtos;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchSts;

public class GetDispatchStsQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetDispatchStsQuery, DispatchStsDto>
{
    public async Task<DispatchStsDto> Handle(GetDispatchStsQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var gantries = await dbContext.Gantries
            .AsNoTracking()
            .OrderBy(g => g.Code)
            .ToListAsync(cancellationToken);

        // Pas de navigation entre agrégats (Gantry/Escale) : jointure explicite pour l'affichage.
        var assignments = await (
            from a in dbContext.GantryAssignments.AsNoTracking()
            join g in dbContext.Gantries.AsNoTracking() on a.GantryId equals g.Id
            join e in dbContext.Escales.AsNoTracking() on a.EscaleId equals e.Id
            orderby a.Statut, a.HeureDebut descending
            select new GantryAssignmentDto
            {
                Id = a.Id,
                GantryId = a.GantryId,
                GantryCode = g.Code,
                EscaleId = a.EscaleId,
                Navire = e.Navire,
                HeureDebut = a.HeureDebut,
                HeureFin = a.HeureFin,
                TacheOuZone = a.TacheOuZone,
                Statut = a.Statut
            }).ToListAsync(cancellationToken);

        var escalesDisponibles = await dbContext.Escales
            .AsNoTracking()
            .Where(e => e.StatutOperations != StatutOperations.Terminees)
            .OrderBy(e => e.Navire)
            .Select(e => new EscaleOptionDto { Id = e.Id, Navire = e.Navire })
            .ToListAsync(cancellationToken);

        var incidentsRaw = await (
            from i in dbContext.StsIncidents.AsNoTracking()
            join e in dbContext.Escales.AsNoTracking() on i.EscaleId equals e.Id
            orderby i.DateDebutUtc descending
            select new { i, e.Navire }).ToListAsync(cancellationToken);

        var gantryCodes = await dbContext.Gantries.AsNoTracking().ToDictionaryAsync(g => g.Id, g => g.Code, cancellationToken);

        var incidents = incidentsRaw.Select(x => new StsIncidentDto
        {
            Id = x.i.Id,
            Navire = x.Navire,
            GantryCode = x.i.GantryId.HasValue && gantryCodes.TryGetValue(x.i.GantryId.Value, out var code) ? code : null,
            TypeIncident = x.i.TypeIncident,
            DateDebutUtc = x.i.DateDebutUtc,
            DateFinUtc = x.i.DateFinUtc,
            Duree = x.i.Duree,
            Cause = x.i.Cause,
            ConditionsReprise = x.i.ConditionsReprise,
            RetirePortiqueEffectif = x.i.RetirePortiqueEffectif,
            EstResolu = x.i.EstResolu
        }).ToList();

        var typesIncident = await dbContext.ReferenceValues
            .AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.StsIncidentType && r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);

        var pointeurs = await dbContext.StsPointeurs
            .AsNoTracking()
            .OrderByDescending(p => p.HeurePriseDePosteUtc)
            .ToListAsync(cancellationToken);

        var ropn = await dbContext.RopnEntries
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        // CDC §6.1 "Sélection du shift" : affichage automatique des navires en cours
        // d'opération ou attendus (ETA) pendant la date/shift sélectionnés.
        var selectedDate = request.Date ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var shiftsDisponibles = await dbContext.ReferenceValues
            .AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.Shift && r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);

        var toutesEscales = await dbContext.Escales
            .AsNoTracking()
            .Where(e => e.StatutOperations != StatutOperations.Terminees)
            .ToListAsync(cancellationToken);

        var naviresDuShift = toutesEscales
            .Where(e => e.StatutOperations == StatutOperations.EnCours
                || (DateOnly.FromDateTime(e.Eta) == selectedDate
                    && (string.IsNullOrEmpty(request.Shift) || string.IsNullOrEmpty(e.Shift) || e.Shift == request.Shift)))
            .OrderBy(e => e.Eta)
            .Select(e => new NavireDuShiftDto
            {
                Id = e.Id,
                Navire = e.Navire,
                Voyage = e.Voyage,
                Eta = e.Eta,
                Shift = e.Shift,
                EnCours = e.StatutOperations == StatutOperations.EnCours
            }).ToList();

        return new DispatchStsDto
        {
            Gantries = gantries.Select(GantryDto.FromEntity).ToList(),
            Assignments = PagedResult<GantryAssignmentDto>.Create(assignments, request.AssignmentsPage),
            EscalesDisponibles = escalesDisponibles,
            Incidents = PagedResult<StsIncidentDto>.Create(incidents, request.IncidentsPage),
            TypesIncidentDisponibles = typesIncident,
            Pointeurs = PagedResult<StsPointeurDto>.Create(pointeurs.Select(StsPointeurDto.FromEntity).ToList(), request.PointeursPage),
            RopnEntries = PagedResult<RopnEntryDto>.Create(ropn.Select(RopnEntryDto.FromEntity).ToList(), request.RopnPage),
            IncidentsEnCoursCount = incidents.Count(i => !i.EstResolu),
            PointeursActifsCount = pointeurs.Count(p => p.HeureFinUtc == null),
            RopnEnCoursCount = ropn.Count(r => r.Statut == RopnStatus.EnCours),
            SelectedDate = selectedDate,
            SelectedShift = request.Shift,
            ShiftsDisponibles = shiftsDisponibles,
            NaviresDuShift = naviresDuShift
        };
    }
}
