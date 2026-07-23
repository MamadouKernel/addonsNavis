using EscaleReport.Web.Application.Cargo.Dtos;
using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Application.Dispatch.Dtos;
using EscaleReport.Web.Application.Dispatch.Queries.GetDispatchTt;
using EscaleReport.Web.Application.Escales.Dtos;
using EscaleReport.Web.Application.VesselPlanning.Dtos;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;

public class GetEscaleDetailQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetEscaleDetailQuery, EscaleDetailDto?>
{
    public async Task<EscaleDetailDto?> Handle(GetEscaleDetailQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var escale = await dbContext.Escales
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.EscaleId, cancellationToken);

        if (escale is null)
        {
            return null;
        }

        var anomalies = await dbContext.ContainerAnomalies
            .AsNoTracking()
            .Where(a => a.EscaleId == request.EscaleId)
            .OrderByDescending(a => a.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var raisons = await dbContext.ReferenceValues
            .AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.AnomalyReason && r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);

        var vides = await dbContext.EmptyContainerTargets
            .AsNoTracking()
            .Where(t => t.EscaleId == request.EscaleId)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var incidents = await dbContext.OperationalIncidents
            .AsNoTracking()
            .Where(i => i.EscaleId == request.EscaleId)
            .OrderByDescending(i => i.DateDebutUtc)
            .ToListAsync(cancellationToken);

        var categoriesIncident = await dbContext.ReferenceValues
            .AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.IncidentCategory && r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);

        var additionnels = await dbContext.AdditionalContainers
            .AsNoTracking()
            .Where(c => c.EscaleId == request.EscaleId)
            .OrderByDescending(c => c.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var dangereux = await dbContext.DangerousContainers
            .AsNoTracking()
            .Where(c => c.EscaleId == request.EscaleId)
            .OrderByDescending(c => c.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        // CDC §14.2 : consommations Cargo, ressources STS/TT et incidents STS de l'escale —
        // repris tels quels depuis leurs modules respectifs, sans ressaisie.
        var cargo = await dbContext.CargoConsommations
            .AsNoTracking()
            .Where(c => c.EscaleId == request.EscaleId)
            .Select(c => CargoConsommationDto.FromEntity(c))
            .FirstOrDefaultAsync(cancellationToken);

        var ressourcesSts = await (
            from a in dbContext.GantryAssignments.AsNoTracking()
            join g in dbContext.Gantries.AsNoTracking() on a.GantryId equals g.Id
            where a.EscaleId == request.EscaleId
            orderby a.HeureDebut descending
            select new GantryAssignmentDto
            {
                Id = a.Id,
                GantryId = a.GantryId,
                GantryCode = g.Code,
                EscaleId = a.EscaleId,
                Navire = escale.Navire,
                HeureDebut = a.HeureDebut,
                HeureFin = a.HeureFin,
                TacheOuZone = a.TacheOuZone,
                Statut = a.Statut
            }).ToListAsync(cancellationToken);

        var ressourcesTt = await dbContext.TtVesselAssignments
            .AsNoTracking()
            .Where(a => a.EscaleId == request.EscaleId)
            .Select(a => new TtVesselAssignmentDto
            {
                Id = a.Id,
                Navire = escale.Navire,
                NombrePrevu = a.NombrePrevu,
                NombreAffecte = a.NombreAffecte,
                NombreOperationnel = a.NombreOperationnel,
                Ecart = a.NombreAffecte - a.NombrePrevu,
                Observations = a.Observations
            }).ToListAsync(cancellationToken);

        var gantryCodes = await dbContext.Gantries.AsNoTracking().ToDictionaryAsync(g => g.Id, g => g.Code, cancellationToken);
        var incidentsSts = await dbContext.StsIncidents
            .AsNoTracking()
            .Where(i => i.EscaleId == request.EscaleId)
            .OrderByDescending(i => i.DateDebutUtc)
            .Select(i => new StsIncidentDto
            {
                Id = i.Id,
                Navire = escale.Navire,
                GantryCode = i.GantryId.HasValue && gantryCodes.ContainsKey(i.GantryId.Value) ? gantryCodes[i.GantryId.Value] : null,
                TypeIncident = i.TypeIncident,
                DateDebutUtc = i.DateDebutUtc,
                DateFinUtc = i.DateFinUtc,
                Duree = i.Duree,
                Cause = i.Cause,
                ConditionsReprise = i.ConditionsReprise,
                RetirePortiqueEffectif = i.RetirePortiqueEffectif,
                EstResolu = i.EstResolu
            }).ToListAsync(cancellationToken);

        var anomaliesDto = anomalies.Select(ContainerAnomalyDto.FromEntity).ToList();
        var videsDto = vides.Select(EmptyContainerTargetDto.FromEntity).ToList();
        var incidentsDto = incidents.Select(OperationalIncidentDto.FromEntity).ToList();
        var additionnelsDto = additionnels.Select(AdditionalContainerDto.FromEntity).ToList();
        var dangereuxDto = dangereux.Select(DangerousContainerDto.FromEntity).ToList();

        // Voir le commentaire sur GetEscaleDetailQuery.Unbounded : la génération PDF/Excel a
        // besoin de tout, pas d'une page de 20 lignes.
        var pageSize = request.Unbounded ? int.MaxValue : Paging.DefaultPageSize;

        return new EscaleDetailDto
        {
            Escale = EscaleDto.FromEntity(escale),
            Anomalies = PagedResult<ContainerAnomalyDto>.Create(anomaliesDto, request.AnomaliesPage, pageSize),
            RaisonsDisponibles = raisons,
            AnomaliesNonResoluesCount = anomaliesDto.Count(a => a.Statut == Domain.VesselPlanning.AnomalyStatus.NonResolu),

            ConteneursVides = PagedResult<EmptyContainerTargetDto>.Create(videsDto, request.VidesPage, pageSize),
            VidesSouhaiteTotal = videsDto.Sum(v => v.QuantiteSouhaitee),
            VidesAjouteTotal = videsDto.Sum(v => v.QuantiteAjoutee),
            VidesPlanifieTotal = videsDto.Sum(v => v.QuantitePlanifiee),
            VidesEmbarqueTotal = videsDto.Sum(v => v.QuantiteEmbarquee),
            VidesCoupeTotal = videsDto.Sum(v => v.QuantiteCoupee),
            VidesResteTotal = videsDto.Sum(v => v.QuantiteRestante),

            Incidents = PagedResult<OperationalIncidentDto>.Create(incidentsDto, request.IncidentsPage, pageSize),
            CategoriesIncidentDisponibles = categoriesIncident,
            IncidentsEnCoursCount = incidentsDto.Count(i => i.Statut == Domain.VesselPlanning.IncidentStatus.EnCours),

            ConteneursAdditionnels = PagedResult<AdditionalContainerDto>.Create(additionnelsDto, request.AdditionnelsPage, pageSize),
            AdditionnelsEnAttenteCount = additionnelsDto.Count(c => c.Decision == Domain.VesselPlanning.AdditionalContainerDecision.EnAttente),

            ConteneursDangereux = PagedResult<DangerousContainerDto>.Create(dangereuxDto, request.DangereuxPage, pageSize),
            DangereuxAlertesCount = dangereuxDto.Count(c => c.AlerteRenouvellement),
            DangereuxNonRegularisesCount = dangereuxDto.Count(c => c.StatutOperationnel == Domain.VesselPlanning.DangerousContainerStatus.ASuivre),

            Cargo = cargo,
            RessourcesSts = ressourcesSts,
            RessourcesTt = ressourcesTt,
            IncidentsSts = incidentsSts
        };
    }
}
