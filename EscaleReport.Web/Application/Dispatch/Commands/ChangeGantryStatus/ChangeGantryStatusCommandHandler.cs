using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.ChangeGantryStatus;

public class ChangeGantryStatusCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ChangeGantryStatusCommand>
{
    public async Task Handle(ChangeGantryStatusCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "STS"))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var gantry = await dbContext.Gantries.FirstOrDefaultAsync(g => g.Id == request.GantryId, cancellationToken);
        if (gantry is null)
        {
            return;
        }

        var ancienStatut = gantry.Statut;
        gantry.ChangerStatut(request.Statut);

        if (request.Statut == GantryStatus.EnPanne &&
            request.EscaleId.HasValue &&
            !string.IsNullOrWhiteSpace(request.TypeIncident))
        {
            var incidentExiste = await dbContext.StsIncidents
                .AnyAsync(i => i.GantryId == request.GantryId && i.DateFinUtc == null, cancellationToken);
            if (!incidentExiste)
            {
                dbContext.StsIncidents.Add(new StsIncident
                {
                    EscaleId = request.EscaleId.Value,
                    GantryId = request.GantryId,
                    TypeIncident = request.TypeIncident.Trim(),
                    Cause = request.Cause,
                    DateDebutUtc = request.DateDebutUtc ?? DateTime.UtcNow,
                    RetirePortiqueEffectif = true
                });
            }
        }
        else if (ancienStatut == GantryStatus.EnPanne && request.Statut != GantryStatus.EnPanne)
        {
            var incidentsOuverts = await dbContext.StsIncidents
                .Where(i => i.GantryId == request.GantryId && i.DateFinUtc == null)
                .ToListAsync(cancellationToken);
            foreach (var incident in incidentsOuverts)
            {
                incident.Cloturer("Reprise automatique après remise en service");
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
