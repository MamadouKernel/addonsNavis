using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseGateTruckIssue;

public record CloseGateTruckIssueCommand(Guid IssueId, string? ActionRealisee) : IRequest;
