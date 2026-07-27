using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.DeleteContainerAnomaly;

public record DeleteContainerAnomalyCommand(Guid AnomalyId) : IRequest;
