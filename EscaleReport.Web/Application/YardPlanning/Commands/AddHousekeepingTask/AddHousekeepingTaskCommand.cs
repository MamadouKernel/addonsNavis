using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Commands.AddHousekeepingTask;

public record AddHousekeepingTaskCommand(
    string Description,
    string? Zone,
    string? Priorite,
    string? Responsable,
    DateTime? DatePrevue) : IRequest<Guid>;
