using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.ResolveContainerAnomaly;

public class ResolveContainerAnomalyCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ResolveContainerAnomalyCommand>
{
    public async Task Handle(ResolveContainerAnomalyCommand request, CancellationToken cancellationToken)
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

        // CDC §5.1 : date/heure de résolution + utilisateur enregistrés automatiquement.
        anomaly.Resoudre(currentUser.UserName);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
