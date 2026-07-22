using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseEnginDeconnexion;

public record CloseEnginDeconnexionCommand(Guid DeconnexionId) : IRequest;
