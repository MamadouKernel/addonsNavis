using MediatR;

namespace EscaleReport.Web.Application.Itt.Commands.AddIttTransferIncident;

public record AddIttTransferIncidentCommand(
    string DifficulteOuObjet,
    DateTime DateDebutUtc,
    string? Note) : IRequest<Guid>;
