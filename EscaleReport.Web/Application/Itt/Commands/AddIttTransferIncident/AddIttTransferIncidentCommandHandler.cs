using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Itt;
using MediatR;

namespace EscaleReport.Web.Application.Itt.Commands.AddIttTransferIncident;

public class AddIttTransferIncidentCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddIttTransferIncidentCommand, Guid>
{
    public async Task<Guid> Handle(AddIttTransferIncidentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var incident = new IttTransferIncident
        {
            DifficulteOuObjet = request.DifficulteOuObjet,
            DateDebutUtc = request.DateDebutUtc,
            Note = request.Note
        };

        dbContext.IttTransferIncidents.Add(incident);
        await dbContext.SaveChangesAsync(cancellationToken);

        return incident.Id;
    }
}
