using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Itt;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Itt.Commands.UpdateIttEquipementEffectif;

public class UpdateIttEquipementEffectifCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateIttEquipementEffectifCommand>
{
    public async Task Handle(UpdateIttEquipementEffectifCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var effectif = await dbContext.IttEquipementEffectifs.FirstOrDefaultAsync(cancellationToken);
        if (effectif is null)
        {
            effectif = new IttEquipementEffectif();
            dbContext.IttEquipementEffectifs.Add(effectif);
        }

        effectif.Disponible = request.Disponible;
        effectif.Engage = request.Engage;
        effectif.EnPanne = request.EnPanne;
        effectif.Observations = request.Observations;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
