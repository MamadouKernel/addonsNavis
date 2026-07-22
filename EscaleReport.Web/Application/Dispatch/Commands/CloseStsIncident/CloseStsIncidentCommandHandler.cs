using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseStsIncident;

public class CloseStsIncidentCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<CloseStsIncidentCommand>
{
    public async Task Handle(CloseStsIncidentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var incident = await dbContext.StsIncidents
            .FirstOrDefaultAsync(i => i.Id == request.IncidentId, cancellationToken);
        if (incident is null)
        {
            return;
        }

        incident.Cloturer(request.ConditionsReprise);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
