using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseStsIncident;

public record CloseStsIncidentCommand(Guid IncidentId, string? ConditionsReprise) : IRequest;
