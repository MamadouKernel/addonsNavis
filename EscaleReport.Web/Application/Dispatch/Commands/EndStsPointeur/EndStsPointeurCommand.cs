using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.EndStsPointeur;

public record EndStsPointeurCommand(Guid PointeurId) : IRequest;
