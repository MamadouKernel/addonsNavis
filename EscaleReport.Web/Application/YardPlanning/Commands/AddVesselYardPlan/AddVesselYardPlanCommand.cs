using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Commands.AddVesselYardPlan;

public record AddVesselYardPlanCommand(
    Guid EscaleId,
    string ServiceMaritime,
    string ZoneDebarquement,
    int ReefersImport,
    int ReefersExport,
    int ConteneursTransbordement,
    string? Observations) : IRequest<Guid>;
