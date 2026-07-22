using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddRopnEntry;

public record AddRopnEntryCommand(string Nom, string? Role, string DifficulteRencontree) : IRequest<Guid>;
