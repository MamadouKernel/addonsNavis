using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.ResolveOperationalIncident;

public record ResolveOperationalIncidentCommand(Guid IncidentId, string? ActionRealisee) : IRequest;
