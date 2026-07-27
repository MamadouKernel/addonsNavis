using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.UpdateOperationalIncident;

public class UpdateOperationalIncidentCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateOperationalIncidentCommand, bool>
{
    public async Task<bool> Handle(UpdateOperationalIncidentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var incident = await dbContext.OperationalIncidents
            .FirstOrDefaultAsync(i => i.Id == request.IncidentId, cancellationToken);
        if (incident is null)
        {
            return true;
        }

        if (request.DateFinUtc.HasValue && request.DateFinUtc.Value < incident.DateDebutUtc)
        {
            return false;
        }

        incident.DateFinUtc = request.DateFinUtc;
        incident.Statut = request.DateFinUtc.HasValue ? IncidentStatus.Resolu : IncidentStatus.EnCours;
        incident.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        incident.ActionRealisee = string.IsNullOrWhiteSpace(request.ActionRealisee) ? null : request.ActionRealisee.Trim();

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
