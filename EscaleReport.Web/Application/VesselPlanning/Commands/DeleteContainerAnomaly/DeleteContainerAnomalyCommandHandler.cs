using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.DeleteContainerAnomaly;

public class DeleteContainerAnomalyCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<DeleteContainerAnomalyCommand>
{
    public async Task Handle(DeleteContainerAnomalyCommand request, CancellationToken cancellationToken)
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

        dbContext.ContainerAnomalies.Remove(anomaly);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
