using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.SetAdditionalContainerDecision;

public record SetAdditionalContainerDecisionCommand(Guid ContainerId, AdditionalContainerDecision Decision) : IRequest;
