using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AssignTt;

public record AssignTtCommand(
    Guid EscaleId,
    int NombrePrevu,
    int NombreAffecte,
    int NombreOperationnel,
    string? Observations) : IRequest<Guid>;
