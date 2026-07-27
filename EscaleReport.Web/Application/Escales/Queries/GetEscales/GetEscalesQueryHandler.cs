using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Application.Escales.Dtos;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Escales;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Escales.Queries.GetEscales;

public class GetEscalesQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetEscalesQuery, PagedResult<EscaleDto>>
{
    public async Task<PagedResult<EscaleDto>> Handle(GetEscalesQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var query = dbContext.Escales
            .AsNoTracking()
            .Where(e => request.Terminees
                ? e.StatutOperations == StatutOperations.Terminees
                : e.StatutOperations != StatutOperations.Terminees);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(e =>
                e.Navire.Contains(search) ||
                (e.Voyage != null && e.Voyage.Contains(search)) ||
                (e.Quai != null && e.Quai.Contains(search)) ||
                (e.LigneMaritime != null && e.LigneMaritime.Contains(search)));
        }

        var orderedQuery = query
            .OrderBy(e => e.StatutOperations == StatutOperations.EnCours ? 0 : 1)
            .ThenBy(e => e.Eta);
        var totalCount = await query.CountAsync(cancellationToken);
        var (page, skip, pageSize, _) = Paging.Resolve(request.Page, request.PageSize, totalCount);

        var escales = await orderedQuery.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);
        var escaleIds = escales.Select(e => e.Id).ToList();

        var anomalyCounts = await dbContext.ContainerAnomalies
            .AsNoTracking()
            .Where(a => escaleIds.Contains(a.EscaleId) && a.Statut == Domain.VesselPlanning.AnomalyStatus.NonResolu)
            .GroupBy(a => a.EscaleId)
            .ToDictionaryAsync(group => group.Key, group => group.Count(), cancellationToken);

        var additionalCounts = await dbContext.AdditionalContainers
            .AsNoTracking()
            .Where(c => escaleIds.Contains(c.EscaleId))
            .GroupBy(c => c.EscaleId)
            .ToDictionaryAsync(group => group.Key, group => group.Count(), cancellationToken);

        return new PagedResult<EscaleDto>
        {
            Items = escales.Select(escale =>
            {
                var dto = EscaleDto.FromEntity(escale);
                dto.AnomaliesNonResoluesCount = anomalyCounts.GetValueOrDefault(escale.Id);
                dto.AdditionnelsCount = additionalCounts.GetValueOrDefault(escale.Id);
                return dto;
            }).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
