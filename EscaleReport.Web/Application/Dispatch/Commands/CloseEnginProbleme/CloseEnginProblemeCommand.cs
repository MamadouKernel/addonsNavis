using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseEnginProbleme;

public record CloseEnginProblemeCommand(Guid ProblemeId, string? ActionRealisee) : IRequest;
