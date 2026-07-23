using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.EndGantryAssignment;

public class EndGantryAssignmentCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<EndGantryAssignmentCommand>
{
    public async Task Handle(EndGantryAssignmentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "STS"))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var assignment = await dbContext.GantryAssignments
            .FirstOrDefaultAsync(a => a.Id == request.AssignmentId, cancellationToken);
        if (assignment is null)
        {
            return;
        }

        assignment.Terminer();

        var gantry = await dbContext.Gantries.FirstOrDefaultAsync(g => g.Id == assignment.GantryId, cancellationToken);
        gantry?.ChangerStatut(GantryStatus.Disponible);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
