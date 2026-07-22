using MediatR;

namespace EscaleReport.Web.Application.Coordination.Commands.CloseCoordinatorIncident;

public record CloseCoordinatorIncidentCommand(Guid IncidentId, string? ActionRealisee) : IRequest;
