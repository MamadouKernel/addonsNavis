using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Commands.AddTransfertOut;

public record AddTransfertOutCommand(
    string Bay,
    int NombreConteneurs,
    string? DetailOuDestination,
    DateTime HeureDebut,
    string? Commentaire) : IRequest<Guid>;
