using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.SetAdditionalContainerDecision;

public class SetAdditionalContainerDecisionCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<SetAdditionalContainerDecisionCommand>
{
    public async Task Handle(SetAdditionalContainerDecisionCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var container = await dbContext.AdditionalContainers
            .FirstOrDefaultAsync(c => c.Id == request.ContainerId, cancellationToken);
        if (container is null)
        {
            return;
        }

        container.EnregistrerDecision(request.Decision, currentUser.UserName);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
