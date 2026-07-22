using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch.Dtos;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchTt;

public class GetDispatchTtQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetDispatchTtQuery, DispatchTtDto>
{
    public async Task<DispatchTtDto> Handle(GetDispatchTtQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var effectif = await dbContext.TtEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        var assignments = await (
            from a in dbContext.TtVesselAssignments.AsNoTracking()
            join e in dbContext.Escales.AsNoTracking() on a.EscaleId equals e.Id
            orderby a.CreatedAtUtc descending
            select new TtVesselAssignmentDto
            {
                Id = a.Id,
                Navire = e.Navire,
                NombrePrevu = a.NombrePrevu,
                NombreAffecte = a.NombreAffecte,
                NombreOperationnel = a.NombreOperationnel,
                Ecart = a.NombreAffecte - a.NombrePrevu,
                Observations = a.Observations
            }).ToListAsync(cancellationToken);

        var escalesDisponibles = await dbContext.Escales
            .AsNoTracking()
            .Where(e => e.StatutOperations != StatutOperations.Terminees)
            .OrderBy(e => e.Navire)
            .Select(e => new EscaleOptionDto { Id = e.Id, Navire = e.Navire })
            .ToListAsync(cancellationToken);

        return new DispatchTtDto
        {
            Effectif = effectif is null
                ? new TtEffectifDto()
                : new TtEffectifDto
                {
                    TotalParc = effectif.TotalParc,
                    Designes = effectif.Designes,
                    NonDesignes = effectif.NonDesignes,
                    RaisonNonDesignation = effectif.RaisonNonDesignation,
                    Disponibles = effectif.Disponibles,
                    Retires = effectif.Retires
                },
            Assignments = assignments,
            EscalesDisponibles = escalesDisponibles
        };
    }
}
