using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.UpdateOperationalIncident;

public record UpdateOperationalIncidentCommand(
    Guid IncidentId,
    DateTime? DateFinUtc,
    string? Description,
    string? ActionRealisee) : IRequest<bool>;
