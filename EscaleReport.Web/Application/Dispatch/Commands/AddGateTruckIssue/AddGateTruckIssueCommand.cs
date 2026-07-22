using EscaleReport.Web.Domain.Dispatch;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddGateTruckIssue;

public record AddGateTruckIssueCommand(
    GateOperationType TypeOperation,
    string CamionReference,
    DateTime DateDebutUtc,
    string ProblemeRencontre) : IRequest<Guid>;
