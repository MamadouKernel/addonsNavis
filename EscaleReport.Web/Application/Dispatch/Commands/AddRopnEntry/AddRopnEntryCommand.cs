using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddRopnEntry;

public record AddRopnEntryCommand(
    string Nom,
    string? Role,
    DateTime DateDebutUtc,
    DateTime? DateFinUtc,
    string DifficulteRencontree) : IRequest<Guid>;
