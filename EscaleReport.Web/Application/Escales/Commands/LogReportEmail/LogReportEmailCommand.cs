using MediatR;

namespace EscaleReport.Web.Application.Escales.Commands.LogReportEmail;

public record LogReportEmailCommand(Guid EscaleId, string Recipients) : IRequest<string?>;
