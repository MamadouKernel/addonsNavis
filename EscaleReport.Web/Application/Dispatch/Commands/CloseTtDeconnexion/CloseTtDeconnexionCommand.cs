using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseTtDeconnexion;

public record CloseTtDeconnexionCommand(Guid DeconnexionId) : IRequest;
