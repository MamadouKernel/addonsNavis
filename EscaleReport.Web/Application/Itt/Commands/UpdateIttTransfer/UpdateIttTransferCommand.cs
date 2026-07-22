using MediatR;

namespace EscaleReport.Web.Application.Itt.Commands.UpdateIttTransfer;

public record UpdateIttTransferCommand(
    Guid Id,
    int NombreATransferer,
    int NombreTransfere,
    int NombreRecu,
    string? Observations) : IRequest;
