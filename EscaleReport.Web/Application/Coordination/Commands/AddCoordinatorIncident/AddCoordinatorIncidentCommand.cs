using MediatR;

namespace EscaleReport.Web.Application.Coordination.Commands.AddCoordinatorIncident;

public record AddCoordinatorIncidentCommand(
    string Objet,
    DateTime DateDebutUtc,
    string? Note) : IRequest<Guid>;
