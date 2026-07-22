using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.ChangeGantryStatus;

public class ChangeGantryStatusCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ChangeGantryStatusCommand>
{
    public async Task Handle(ChangeGantryStatusCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var gantry = await dbContext.Gantries.FirstOrDefaultAsync(g => g.Id == request.GantryId, cancellationToken);
        if (gantry is null)
        {
            return;
        }

        gantry.ChangerStatut(request.Statut);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
