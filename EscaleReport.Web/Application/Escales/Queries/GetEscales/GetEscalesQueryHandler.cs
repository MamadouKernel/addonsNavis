using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Application.Escales.Dtos;
using EscaleReport.Web.Domain.Identity;
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

        var query = dbContext.Escales.AsNoTracking().OrderByDescending(e => e.Eta);
        var totalCount = await query.CountAsync(cancellationToken);
        var (page, skip, pageSize, _) = Paging.Resolve(request.Page, request.PageSize, totalCount);

        var escales = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedResult<EscaleDto>
        {
            Items = escales.Select(EscaleDto.FromEntity).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
