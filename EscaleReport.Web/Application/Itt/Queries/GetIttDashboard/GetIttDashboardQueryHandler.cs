using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Itt.Queries.GetIttDashboard;

public class GetIttDashboardQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetIttDashboardQuery, IttDashboardDto>
{
    public async Task<IttDashboardDto> Handle(GetIttDashboardQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var transfers = await dbContext.IttTransfers
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAtUtc)
            .Select(t => new IttTransferDto
            {
                Id = t.Id,
                NavireConnexion = t.NavireConnexion,
                NombreATransferer = t.NombreATransferer,
                NombreTransfere = t.NombreTransfere,
                NombreRecu = t.NombreRecu,
                NombreRestant = t.NombreRestant,
                PourcentageAvancement = t.PourcentageAvancement,
                Observations = t.Observations
            }).ToListAsync(cancellationToken);

        var incidents = await dbContext.IttTransferIncidents
            .AsNoTracking()
            .OrderByDescending(i => i.DateDebutUtc)
            .Select(i => new IttTransferIncidentDto
            {
                Id = i.Id,
                DifficulteOuObjet = i.DifficulteOuObjet,
                DateDebutUtc = i.DateDebutUtc,
                DateFinUtc = i.DateFinUtc,
                Duree = i.Duree,
                Note = i.Note,
                ActionRealisee = i.ActionRealisee,
                EstResolu = i.EstResolu
            }).ToListAsync(cancellationToken);

        var effectif = await dbContext.IttEquipementEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        var pannes = await dbContext.IttEnginPannes
            .AsNoTracking()
            .OrderByDescending(p => p.DateDebutUtc)
            .Select(p => new IttEnginPanneDto
            {
                Id = p.Id,
                Engin = p.Engin,
                DateDebutUtc = p.DateDebutUtc,
                DateFinUtc = p.DateFinUtc,
                Duree = p.Duree,
                Cause = p.Cause,
                RetireEffectif = p.RetireEffectif,
                ActionRealisee = p.ActionRealisee,
                EstResolue = p.EstResolue
            }).ToListAsync(cancellationToken);

        return new IttDashboardDto
        {
            Transfers = transfers,
            TransferIncidents = incidents,
            EquipementEffectif = effectif is null
                ? new IttEquipementEffectifDto()
                : new IttEquipementEffectifDto
                {
                    Disponible = effectif.Disponible,
                    Engage = effectif.Engage,
                    EnPanne = effectif.EnPanne,
                    Observations = effectif.Observations
                },
            EnginPannes = pannes
        };
    }
}
