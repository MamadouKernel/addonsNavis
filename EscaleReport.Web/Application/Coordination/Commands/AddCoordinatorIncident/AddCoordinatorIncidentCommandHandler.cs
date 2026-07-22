using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Coordination;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Coordination.Commands.AddCoordinatorIncident;

public class AddCoordinatorIncidentCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddCoordinatorIncidentCommand, Guid>
{
    public async Task<Guid> Handle(AddCoordinatorIncidentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var incident = new CoordinatorIncident
        {
            Objet = request.Objet,
            DateDebutUtc = request.DateDebutUtc,
            Note = request.Note
        };

        dbContext.CoordinatorIncidents.Add(incident);
        await dbContext.SaveChangesAsync(cancellationToken);

        return incident.Id;
    }
}
