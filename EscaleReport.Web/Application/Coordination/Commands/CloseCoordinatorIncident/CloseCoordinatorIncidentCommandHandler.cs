using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Coordination.Commands.CloseCoordinatorIncident;

public class CloseCoordinatorIncidentCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<CloseCoordinatorIncidentCommand>
{
    public async Task Handle(CloseCoordinatorIncidentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var incident = await dbContext.CoordinatorIncidents
            .FirstOrDefaultAsync(i => i.Id == request.IncidentId, cancellationToken);
        if (incident is null)
        {
            return;
        }

        incident.Cloturer(request.ActionRealisee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
