using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Commands.EndTransfertOut;

public record EndTransfertOutCommand(Guid TransfertId) : IRequest;
