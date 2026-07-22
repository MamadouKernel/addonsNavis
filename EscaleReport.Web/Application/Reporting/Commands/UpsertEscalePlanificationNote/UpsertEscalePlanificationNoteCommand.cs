using MediatR;

namespace EscaleReport.Web.Application.Reporting.Commands.UpsertEscalePlanificationNote;

public record UpsertEscalePlanificationNoteCommand(Guid EscaleId, string? Commentaire) : IRequest;
