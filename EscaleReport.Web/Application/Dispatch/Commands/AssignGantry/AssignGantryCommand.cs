using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AssignGantry;

public record AssignGantryCommand(
    Guid GantryId,
    Guid EscaleId,
    DateTime? HeureDebut,
    string? TacheOuZone) : IRequest<Guid>;
