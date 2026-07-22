using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseRtgPanne;

public record CloseRtgPanneCommand(Guid PanneId, string? CommentaireReprise) : IRequest;
