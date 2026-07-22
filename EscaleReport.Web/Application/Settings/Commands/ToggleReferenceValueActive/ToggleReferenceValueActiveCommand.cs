using MediatR;

namespace EscaleReport.Web.Application.Settings.Commands.ToggleReferenceValueActive;

public record ToggleReferenceValueActiveCommand(Guid Id) : IRequest;
