using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.YardPlanning.Commands.ChangeHousekeepingTaskStatus;

public class ChangeHousekeepingTaskStatusCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ChangeHousekeepingTaskStatusCommand>
{
    public async Task Handle(ChangeHousekeepingTaskStatusCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var task = await dbContext.HousekeepingTasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);
        if (task is null)
        {
            return;
        }

        task.ChangerStatut(request.Statut);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
