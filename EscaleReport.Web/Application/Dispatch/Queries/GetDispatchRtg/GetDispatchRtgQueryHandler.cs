using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Application.Dispatch.Dtos;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchRtg;

public class GetDispatchRtgQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetDispatchRtgQuery, DispatchRtgDto>
{
    public async Task<DispatchRtgDto> Handle(GetDispatchRtgQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var effectif = await dbContext.RtgEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        var pannes = await dbContext.RtgPannes
            .AsNoTracking()
            .OrderByDescending(p => p.DateDebutUtc)
            .Select(p => new RtgPanneDto
            {
                Id = p.Id,
                Engin = p.Engin,
                DateDebutUtc = p.DateDebutUtc,
                DateFinUtc = p.DateFinUtc,
                Duree = p.Duree,
                Raison = p.Raison,
                RetireEffectif = p.RetireEffectif,
                CommentaireReprise = p.CommentaireReprise,
                EstResolue = p.EstResolue
            }).ToListAsync(cancellationToken);

        var clashes = await dbContext.RtgClashes
            .AsNoTracking()
            .OrderByDescending(c => c.DateDebutUtc)
            .Select(c => new RtgClashDto
            {
                Id = c.Id,
                Lieu = c.Lieu,
                EnginsConcernes = c.EnginsConcernes,
                DateDebutUtc = c.DateDebutUtc,
                DateFinUtc = c.DateFinUtc,
                Duree = c.Duree,
                Description = c.Description,
                ActionRealisee = c.ActionRealisee,
                EstResolu = c.EstResolu
            }).ToListAsync(cancellationToken);

        var gateIssues = await dbContext.GateTruckIssues
            .AsNoTracking()
            .OrderByDescending(g => g.DateDebutUtc)
            .Select(g => new GateTruckIssueDto
            {
                Id = g.Id,
                TypeOperation = g.TypeOperation,
                CamionReference = g.CamionReference,
                DateDebutUtc = g.DateDebutUtc,
                DateFinUtc = g.DateFinUtc,
                Duree = g.Duree,
                ProblemeRencontre = g.ProblemeRencontre,
                ActionRealisee = g.ActionRealisee,
                EstResolu = g.EstResolu
            }).ToListAsync(cancellationToken);

        // CDC §8.2 : consultation en lecture seule des incidents STS du shift (même jointure
        // que dans GetDispatchStsQueryHandler, pas de navigation entre agrégats).
        var incidentsRaw = await (
            from i in dbContext.StsIncidents.AsNoTracking()
            join e in dbContext.Escales.AsNoTracking() on i.EscaleId equals e.Id
            orderby i.DateDebutUtc descending
            select new { i, e.Navire }).ToListAsync(cancellationToken);

        var gantryCodes = await dbContext.Gantries.AsNoTracking().ToDictionaryAsync(g => g.Id, g => g.Code, cancellationToken);

        var stsIncidents = incidentsRaw.Select(x => new StsIncidentDto
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

        return new DispatchRtgDto
        {
            Effectif = effectif is null
                ? new RtgEffectifDto()
                : new RtgEffectifDto
                {
                    TotalParc = effectif.TotalParc,
                    Disponible = effectif.Disponible,
                    Affecte = effectif.Affecte,
                    EnPanne = effectif.EnPanne,
                    Retire = effectif.Retire
                },
            Pannes = PagedResult<RtgPanneDto>.Create(pannes, request.PannesPage),
            Clashes = PagedResult<RtgClashDto>.Create(clashes, request.ClashesPage),
            GateTruckIssues = PagedResult<GateTruckIssueDto>.Create(gateIssues, request.GateTruckIssuesPage),
            StsIncidents = PagedResult<StsIncidentDto>.Create(stsIncidents, request.StsIncidentsPage),
            PannesEnCoursCount = pannes.Count(p => !p.EstResolue),
            ClashsEnCoursCount = clashes.Count(c => !c.EstResolu),
            GateEnCoursCount = gateIssues.Count(g => !g.EstResolu),
            IncidentsStsEnCoursCount = stsIncidents.Count(i => !i.EstResolu)
        };
    }
}
