using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseGateTruckIssue;

public class CloseGateTruckIssueCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<CloseGateTruckIssueCommand>
{
    public async Task Handle(CloseGateTruckIssueCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "RTG"))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var issue = await dbContext.GateTruckIssues
            .FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);
        if (issue is null)
        {
            return;
        }

        issue.Cloturer(request.ActionRealisee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
