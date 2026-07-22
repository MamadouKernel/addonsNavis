using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.ResolveRopnEntry;

public record ResolveRopnEntryCommand(Guid RopnEntryId, string? ActionRealisee) : IRequest;
