using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Settings.Commands.UpsertAlertThreshold;

public class UpsertAlertThresholdCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpsertAlertThresholdCommand>
{
    public async Task Handle(UpsertAlertThresholdCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierParametres))
        {
            throw new ForbiddenAccessException(Permissions.ModifierParametres);
        }

        var threshold = await dbContext.AlertThresholds.FirstOrDefaultAsync(t => t.Cle == request.Cle, cancellationToken);
        if (threshold is null)
        {
            threshold = new AlertThreshold { Cle = request.Cle };
            dbContext.AlertThresholds.Add(threshold);
        }

        threshold.Libelle = request.Libelle;
        threshold.ValeurHeures = request.ValeurHeures;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
