using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.AssignGantry;

public class AssignGantryCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AssignGantryCommand, Guid>
{
    public async Task<Guid> Handle(AssignGantryCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "STS"))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var assignment = new GantryAssignment
        {
            GantryId = request.GantryId,
            EscaleId = request.EscaleId,
            HeureDebut = request.HeureDebut,
            TacheOuZone = request.TacheOuZone
        };
        dbContext.GantryAssignments.Add(assignment);

        // Le statut du portique reflète l'affectation en cours (CDC §6.2).
        var gantry = await dbContext.Gantries.FirstAsync(g => g.Id == request.GantryId, cancellationToken);
        gantry.ChangerStatut(GantryStatus.Affecte);

        await dbContext.SaveChangesAsync(cancellationToken);

        return assignment.Id;
    }
}
