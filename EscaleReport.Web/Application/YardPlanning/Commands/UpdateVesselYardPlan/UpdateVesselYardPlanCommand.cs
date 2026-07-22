using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Commands.UpdateVesselYardPlan;

public record UpdateVesselYardPlanCommand(
    Guid Id,
    int ReefersImport,
    int ReefersExport,
    int ConteneursTransbordement,
    string? Observations) : IRequest;
