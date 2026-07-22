using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.ResolveOperationalIncident;

public class ResolveOperationalIncidentCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ResolveOperationalIncidentCommand>
{
    public async Task Handle(ResolveOperationalIncidentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var incident = await dbContext.OperationalIncidents
            .FirstOrDefaultAsync(i => i.Id == request.IncidentId, cancellationToken);
        if (incident is null)
        {
            return;
        }

        incident.Resoudre(request.ActionRealisee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
