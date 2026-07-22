using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Escales.Dtos;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Escales.Queries.GetEscales;

public class GetEscalesQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetEscalesQuery, IReadOnlyList<EscaleDto>>
{
    public async Task<IReadOnlyList<EscaleDto>> Handle(GetEscalesQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var escales = await dbContext.Escales
            .AsNoTracking()
            .OrderByDescending(e => e.Eta)
            .ToListAsync(cancellationToken);

        return escales.Select(EscaleDto.FromEntity).ToList();
    }
}
