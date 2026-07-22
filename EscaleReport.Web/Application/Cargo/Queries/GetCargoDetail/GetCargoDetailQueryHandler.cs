using EscaleReport.Web.Application.Cargo.Dtos;
using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Escales.Dtos;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Cargo.Queries.GetCargoDetail;

public class GetCargoDetailQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetCargoDetailQuery, CargoDetailDto?>
{
    public async Task<CargoDetailDto?> Handle(GetCargoDetailQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var escale = await dbContext.Escales.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.EscaleId, cancellationToken);
        if (escale is null)
        {
            return null;
        }

        var consommation = await dbContext.CargoConsommations.AsNoTracking()
            .FirstOrDefaultAsync(c => c.EscaleId == request.EscaleId, cancellationToken);

        return new CargoDetailDto
        {
            Escale = EscaleDto.FromEntity(escale),
            Consommation = consommation is null
                ? new CargoConsommationDto()
                : CargoConsommationDto.FromEntity(consommation)
        };
    }
}
