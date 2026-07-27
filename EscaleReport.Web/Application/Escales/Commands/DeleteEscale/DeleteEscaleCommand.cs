using MediatR;

namespace EscaleReport.Web.Application.Escales.Commands.DeleteEscale;

public record DeleteEscaleCommand(Guid Id) : IRequest;
