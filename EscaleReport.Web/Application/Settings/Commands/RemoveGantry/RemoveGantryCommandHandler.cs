using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Settings.Commands.RemoveGantry;

public class RemoveGantryCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<RemoveGantryCommand>
{
    public async Task Handle(RemoveGantryCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierParametres))
        {
            throw new ForbiddenAccessException(Permissions.ModifierParametres);
        }

        var gantry = await dbContext.Gantries.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
        if (gantry is null)
        {
            return;
        }

        var isReferenced = await dbContext.GantryAssignments.AnyAsync(a => a.GantryId == request.Id, cancellationToken)
            || await dbContext.StsIncidents.AnyAsync(i => i.GantryId == request.Id, cancellationToken);
        if (isReferenced)
        {
            // Un portique déjà utilisé dans une affectation ou un incident STS ne peut pas être
            // supprimé sans casser l'historique — l'écran de paramétrage masque déjà l'action
            // dans ce cas, ce contrôle serveur est la garantie réelle.
            return;
        }

        dbContext.Gantries.Remove(gantry);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
