using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.UpdateEmptyContainerTarget;

public class UpdateEmptyContainerTargetCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateEmptyContainerTargetCommand>
{
    public async Task Handle(UpdateEmptyContainerTargetCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var target = await dbContext.EmptyContainerTargets
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (target is null)
        {
            return;
        }

        target.QuantiteAjoutee = request.QuantiteAjoutee;
        target.QuantitePlanifiee = request.QuantitePlanifiee;
        target.QuantiteEmbarquee = request.QuantiteEmbarquee;
        target.QuantiteCoupee = request.QuantiteCoupee;
        target.MotifCoupure = request.MotifCoupure;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
