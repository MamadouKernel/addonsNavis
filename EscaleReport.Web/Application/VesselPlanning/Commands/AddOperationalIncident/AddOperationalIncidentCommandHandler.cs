using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddOperationalIncident;

public class AddOperationalIncidentCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddOperationalIncidentCommand, Guid>
{
    public async Task<Guid> Handle(AddOperationalIncidentCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var incident = new Domain.VesselPlanning.OperationalIncident
        {
            EscaleId = request.EscaleId,
            Categorie = request.Categorie,
            Localisation = request.Localisation,
            DateDebutUtc = request.DateDebutUtc,
            Gravite = request.Gravite,
            Description = request.Description,
            DeclarePar = currentUser.UserName
        };

        dbContext.OperationalIncidents.Add(incident);
        await dbContext.SaveChangesAsync(cancellationToken);

        // CDC §5.3 : un incident critique doit être transmis au Coordinateur de la Control
        // Room — la Vue consolidée du Coordinateur (déjà bâtie sur une lecture directe des
        // incidents) le reflète immédiatement, sans file d'attente de notification séparée
        // à ce stade du périmètre.

        return incident.Id;
    }
}
