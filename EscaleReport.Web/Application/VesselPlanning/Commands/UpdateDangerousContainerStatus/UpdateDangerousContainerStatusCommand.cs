using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.UpdateDangerousContainerStatus;

public record UpdateDangerousContainerStatusCommand(
    Guid ContainerId,
    BadtStatus StatutBadt,
    DangerousContainerStatus StatutOperationnel) : IRequest;
