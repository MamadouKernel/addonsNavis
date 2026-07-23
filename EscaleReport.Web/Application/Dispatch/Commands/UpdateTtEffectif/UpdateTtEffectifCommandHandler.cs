using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateTtEffectif;

public class UpdateTtEffectifCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateTtEffectifCommand>
{
    public async Task Handle(UpdateTtEffectifCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "TT"))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        // Effectif "courant" unique (pas d'historique par shift dans ce périmètre) —
        // même simplification que pour le pool de portiques STS.
        var effectif = await dbContext.TtEffectifs.FirstOrDefaultAsync(cancellationToken);
        if (effectif is null)
        {
            effectif = new TtEffectif();
            dbContext.TtEffectifs.Add(effectif);
        }

        effectif.TotalParc = request.TotalParc;
        effectif.Designes = request.Designes;
        effectif.RaisonNonDesignation = request.RaisonNonDesignation;
        effectif.Retires = request.Retires;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
