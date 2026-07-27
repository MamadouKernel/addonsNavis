using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.UpdateContainerAnomalyPosition;

public class UpdateContainerAnomalyPositionCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateContainerAnomalyPositionCommand>
{
    public async Task Handle(UpdateContainerAnomalyPositionCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var anomaly = await dbContext.ContainerAnomalies
            .FirstOrDefaultAsync(a => a.Id == request.AnomalyId, cancellationToken);
        if (anomaly is null)
        {
            return;
        }

        anomaly.Position = string.IsNullOrWhiteSpace(request.Position) ? null : request.Position.Trim();
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
