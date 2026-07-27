using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateGantryAssignment;

public record UpdateGantryAssignmentCommand(
    Guid AssignmentId,
    DateTime HeureDebut,
    DateTime? HeureFin) : IRequest;
