using MediatR;

namespace EscaleReport.Web.Application.Settings.Commands.RemoveAlertThreshold;

public record RemoveAlertThresholdCommand(Guid Id) : IRequest;
