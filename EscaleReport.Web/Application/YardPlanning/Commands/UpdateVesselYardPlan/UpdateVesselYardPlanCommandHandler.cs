using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.YardPlanning.Commands.UpdateVesselYardPlan;

public class UpdateVesselYardPlanCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateVesselYardPlanCommand>
{
    public async Task Handle(UpdateVesselYardPlanCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var plan = await dbContext.VesselYardPlans
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (plan is null)
        {
            return;
        }

        plan.ReefersImport = request.ReefersImport;
        plan.ReefersExport = request.ReefersExport;
        plan.ConteneursTransbordement = request.ConteneursTransbordement;
        plan.Observations = request.Observations;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
