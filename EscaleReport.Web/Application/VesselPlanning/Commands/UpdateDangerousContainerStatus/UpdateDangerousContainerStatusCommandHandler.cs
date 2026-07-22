using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.UpdateDangerousContainerStatus;

public class UpdateDangerousContainerStatusCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateDangerousContainerStatusCommand>
{
    public async Task Handle(UpdateDangerousContainerStatusCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var container = await dbContext.DangerousContainers
            .FirstOrDefaultAsync(c => c.Id == request.ContainerId, cancellationToken);
        if (container is null)
        {
            return;
        }

        container.StatutBadt = request.StatutBadt;
        container.StatutOperationnel = request.StatutOperationnel;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
