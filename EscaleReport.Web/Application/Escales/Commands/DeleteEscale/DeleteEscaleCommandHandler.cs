using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Escales.Commands.DeleteEscale;

public class DeleteEscaleCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<DeleteEscaleCommand>
{
    public async Task Handle(DeleteEscaleCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SupprimerEscale))
        {
            throw new ForbiddenAccessException(Permissions.SupprimerEscale);
        }

        var escale = await dbContext.Escales
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        if (escale is null)
        {
            return;
        }

        // Le soft-delete du parent ne déclenche pas les cascades SQL. On charge donc
        // explicitement les registres de l'agrégat afin que l'intercepteur les masque
        // aussi, tout en libérant les ressources physiques encore affectées.
        var gantryAssignments = await dbContext.GantryAssignments
            .Where(x => x.EscaleId == request.Id)
            .ToListAsync(cancellationToken);
        var activeGantryIds = gantryAssignments
            .Where(x => x.Statut == Domain.Dispatch.AssignmentStatus.EnCours)
            .Select(x => x.GantryId)
            .Distinct()
            .ToList();
        if (activeGantryIds.Count > 0)
        {
            var gantries = await dbContext.Gantries
                .Where(x => activeGantryIds.Contains(x.Id))
                .ToListAsync(cancellationToken);
            foreach (var gantry in gantries)
            {
                gantry.ChangerStatut(Domain.Dispatch.GantryStatus.Disponible);
            }
        }

        dbContext.GantryAssignments.RemoveRange(gantryAssignments);
        dbContext.StsIncidents.RemoveRange(await dbContext.StsIncidents.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.TtVesselAssignments.RemoveRange(await dbContext.TtVesselAssignments.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.ContainerAnomalies.RemoveRange(await dbContext.ContainerAnomalies.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.EmptyContainerTargets.RemoveRange(await dbContext.EmptyContainerTargets.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.OperationalIncidents.RemoveRange(await dbContext.OperationalIncidents.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.AdditionalContainers.RemoveRange(await dbContext.AdditionalContainers.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.DangerousContainers.RemoveRange(await dbContext.DangerousContainers.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.CargoConsommations.RemoveRange(await dbContext.CargoConsommations.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.ReportEmailLogs.RemoveRange(await dbContext.ReportEmailLogs.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.VesselYardPlans.RemoveRange(await dbContext.VesselYardPlans.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.EscalePlanificationNotes.RemoveRange(await dbContext.EscalePlanificationNotes.Where(x => x.EscaleId == request.Id).ToListAsync(cancellationToken));
        dbContext.Escales.Remove(escale);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
