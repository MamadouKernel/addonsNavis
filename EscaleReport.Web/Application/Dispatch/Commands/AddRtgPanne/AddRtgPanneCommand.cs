using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddRtgPanne;

public record AddRtgPanneCommand(
    string Engin,
    DateTime DateDebutUtc,
    string? Raison,
    bool RetireEffectif) : IRequest<Guid>;
