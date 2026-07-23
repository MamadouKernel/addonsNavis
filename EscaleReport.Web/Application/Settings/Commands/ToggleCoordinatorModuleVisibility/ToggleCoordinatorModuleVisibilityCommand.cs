using MediatR;

namespace EscaleReport.Web.Application.Settings.Commands.ToggleCoordinatorModuleVisibility;

public record ToggleCoordinatorModuleVisibilityCommand(string Cle) : IRequest;
