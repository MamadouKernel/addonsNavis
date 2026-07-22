using MediatR;

namespace EscaleReport.Web.Application.Itt.Commands.AddIttEnginPanne;

public record AddIttEnginPanneCommand(
    string Engin,
    DateTime DateDebutUtc,
    string? Cause,
    bool RetireEffectif) : IRequest<Guid>;
