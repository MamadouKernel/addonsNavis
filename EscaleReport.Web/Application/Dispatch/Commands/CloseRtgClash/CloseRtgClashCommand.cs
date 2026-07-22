using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseRtgClash;

public record CloseRtgClashCommand(Guid ClashId, string? ActionRealisee) : IRequest;
