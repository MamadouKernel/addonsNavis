using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Settings.Commands.RemoveAlertThreshold;

public class RemoveAlertThresholdCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<RemoveAlertThresholdCommand>
{
    public async Task Handle(RemoveAlertThresholdCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierParametres))
        {
            throw new ForbiddenAccessException(Permissions.ModifierParametres);
        }

        var threshold = await dbContext.AlertThresholds.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (threshold is null)
        {
            return;
        }

        dbContext.AlertThresholds.Remove(threshold);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
