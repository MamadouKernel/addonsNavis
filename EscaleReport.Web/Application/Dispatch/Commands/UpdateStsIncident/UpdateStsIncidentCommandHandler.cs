using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateStsIncident;

public class UpdateStsIncidentCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateStsIncidentCommand>
{
    public async Task Handle(UpdateStsIncidentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "STS"))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var incident = await dbContext.StsIncidents
            .FirstOrDefaultAsync(i => i.Id == request.IncidentId, cancellationToken);
        if (incident is null)
        {
            return;
        }

        incident.MettreAJour(
            request.EscaleId,
            request.GantryId,
            request.TypeIncident,
            request.DateDebutUtc,
            request.DateFinUtc,
            request.Cause,
            request.ConditionsReprise,
            request.RetirePortiqueEffectif);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
