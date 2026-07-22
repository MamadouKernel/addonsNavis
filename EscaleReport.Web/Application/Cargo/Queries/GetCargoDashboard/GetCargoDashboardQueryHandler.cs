using EscaleReport.Web.Application.Cargo.Dtos;
using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Cargo.Queries.GetCargoDashboard;

public class GetCargoDashboardQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetCargoDashboardQuery, IReadOnlyList<CargoDashboardRowDto>>
{
    public async Task<IReadOnlyList<CargoDashboardRowDto>> Handle(GetCargoDashboardQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        // Jointure gauche : une escale peut ne pas avoir encore de consolidation Cargo saisie.
        var rows = await (
            from e in dbContext.Escales.AsNoTracking()
            join c in dbContext.CargoConsommations.AsNoTracking() on e.Id equals c.EscaleId into cargo
            from c in cargo.DefaultIfEmpty()
            orderby e.Eta descending
            select new CargoDashboardRowDto
            {
                EscaleId = e.Id,
                Navire = e.Navire,
                Voyage = e.Voyage,
                DischRealisee = c != null && c.DischRealisee,
                LoadRealisee = c != null && c.LoadRealisee,
                RevisedLoadRecu = c != null && c.RevisedLoadRecu
            }).ToListAsync(cancellationToken);

        return rows;
    }
}
