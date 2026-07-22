using EscaleReport.Web.Domain.YardPlanning;
using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Commands.ChangeHousekeepingTaskStatus;

public record ChangeHousekeepingTaskStatusCommand(Guid TaskId, HousekeepingStatus Statut) : IRequest;
