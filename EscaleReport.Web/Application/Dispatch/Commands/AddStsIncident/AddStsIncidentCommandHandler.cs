using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddStsIncident;

public class AddStsIncidentCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddStsIncidentCommand, Guid>
{
    public async Task<Guid> Handle(AddStsIncidentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "STS"))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var incident = new StsIncident
        {
            EscaleId = request.EscaleId,
            GantryId = request.GantryId,
            TypeIncident = request.TypeIncident,
            DateDebutUtc = request.DateDebutUtc,
            Cause = request.Cause,
            RetirePortiqueEffectif = request.RetirePortiqueEffectif
        };

        dbContext.StsIncidents.Add(incident);
        await dbContext.SaveChangesAsync(cancellationToken);

        return incident.Id;
    }
}
