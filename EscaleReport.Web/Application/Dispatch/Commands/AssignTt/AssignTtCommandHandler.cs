using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AssignTt;

public class AssignTtCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AssignTtCommand, Guid>
{
    public async Task<Guid> Handle(AssignTtCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var assignment = new TtVesselAssignment
        {
            EscaleId = request.EscaleId,
            NombrePrevu = request.NombrePrevu,
            NombreAffecte = request.NombreAffecte,
            NombreOperationnel = request.NombreOperationnel,
            Observations = request.Observations
        };

        dbContext.TtVesselAssignments.Add(assignment);
        await dbContext.SaveChangesAsync(cancellationToken);

        return assignment.Id;
    }
}
