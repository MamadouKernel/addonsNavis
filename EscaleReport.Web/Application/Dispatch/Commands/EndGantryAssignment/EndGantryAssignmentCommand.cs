using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.EndGantryAssignment;

public record EndGantryAssignmentCommand(Guid AssignmentId, DateTime? HeureFin = null) : IRequest;
