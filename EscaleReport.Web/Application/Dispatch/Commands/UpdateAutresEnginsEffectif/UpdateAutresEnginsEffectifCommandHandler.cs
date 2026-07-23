using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateAutresEnginsEffectif;

public class UpdateAutresEnginsEffectifCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateAutresEnginsEffectifCommand>
{
    public async Task Handle(UpdateAutresEnginsEffectifCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "Autres engins"))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        // Effectif "courant" unique (même simplification que RtgEffectif/TtEffectif).
        var effectif = await dbContext.AutresEnginsEffectifs.FirstOrDefaultAsync(cancellationToken);
        if (effectif is null)
        {
            effectif = new AutresEnginsEffectif();
            dbContext.AutresEnginsEffectifs.Add(effectif);
        }

        effectif.DisponibleReachStackers = request.DisponibleReachStackers;
        effectif.DisponibleEmptyHandlers = request.DisponibleEmptyHandlers;
        effectif.DisponibleAutres = request.DisponibleAutres;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
