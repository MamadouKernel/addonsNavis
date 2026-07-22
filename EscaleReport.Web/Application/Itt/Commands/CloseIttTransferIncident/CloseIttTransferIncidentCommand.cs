using MediatR;

namespace EscaleReport.Web.Application.Itt.Commands.CloseIttTransferIncident;

public record CloseIttTransferIncidentCommand(Guid IncidentId, string? ActionRealisee) : IRequest;
