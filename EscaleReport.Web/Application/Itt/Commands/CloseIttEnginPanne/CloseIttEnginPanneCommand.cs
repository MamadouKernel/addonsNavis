using MediatR;

namespace EscaleReport.Web.Application.Itt.Commands.CloseIttEnginPanne;

public record CloseIttEnginPanneCommand(Guid PanneId, string? ActionRealisee) : IRequest;
