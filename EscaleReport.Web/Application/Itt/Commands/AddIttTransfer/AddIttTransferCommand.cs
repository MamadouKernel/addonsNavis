using MediatR;

namespace EscaleReport.Web.Application.Itt.Commands.AddIttTransfer;

public record AddIttTransferCommand(
    string NavireConnexion,
    int NombreATransferer,
    int NombreTransfere,
    int NombreRecu,
    string? Observations) : IRequest<Guid>;
