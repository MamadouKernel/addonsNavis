using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.YardPlanning;
using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Commands.AddVesselYardPlan;

public class AddVesselYardPlanCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddVesselYardPlanCommand, Guid>
{
    public async Task<Guid> Handle(AddVesselYardPlanCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var plan = new VesselYardPlan
        {
            EscaleId = request.EscaleId,
            ServiceMaritime = request.ServiceMaritime,
            ZoneDebarquement = request.ZoneDebarquement,
            ReefersImport = request.ReefersImport,
            ReefersExport = request.ReefersExport,
            ConteneursTransbordement = request.ConteneursTransbordement,
            Observations = request.Observations
        };

        dbContext.VesselYardPlans.Add(plan);
        await dbContext.SaveChangesAsync(cancellationToken);

        return plan.Id;
    }
}
