using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.UpdateContainerAnomalyPosition;

public record UpdateContainerAnomalyPositionCommand(
    Guid AnomalyId,
    string? Position) : IRequest;
