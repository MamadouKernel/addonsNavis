using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.ResolveContainerAnomaly;

public record ResolveContainerAnomalyCommand(Guid AnomalyId) : IRequest;
