using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateRtgEffectif;

public class UpdateRtgEffectifCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateRtgEffectifCommand>
{
    public async Task Handle(UpdateRtgEffectifCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        // Effectif "courant" unique (pas d'historique par shift dans ce périmètre) —
        // même simplification que pour les pools de portiques STS et de TT.
        var effectif = await dbContext.RtgEffectifs.FirstOrDefaultAsync(cancellationToken);
        if (effectif is null)
        {
            effectif = new RtgEffectif();
            dbContext.RtgEffectifs.Add(effectif);
        }

        effectif.TotalParc = request.TotalParc;
        effectif.Disponible = request.Disponible;
        effectif.Affecte = request.Affecte;
        effectif.EnPanne = request.EnPanne;
        effectif.Retire = request.Retire;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
