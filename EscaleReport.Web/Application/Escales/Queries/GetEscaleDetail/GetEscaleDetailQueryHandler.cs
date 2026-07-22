using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
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

        return new EscaleDetailDto
        {
            Escale = EscaleDto.FromEntity(escale),
            Anomalies = anomalies.Select(ContainerAnomalyDto.FromEntity).ToList(),
            RaisonsDisponibles = raisons,
            ConteneursVides = vides.Select(EmptyContainerTargetDto.FromEntity).ToList(),
            Incidents = incidents.Select(OperationalIncidentDto.FromEntity).ToList(),
            CategoriesIncidentDisponibles = categoriesIncident,
            ConteneursAdditionnels = additionnels.Select(AdditionalContainerDto.FromEntity).ToList(),
            ConteneursDangereux = dangereux.Select(DangerousContainerDto.FromEntity).ToList()
        };
    }
}
