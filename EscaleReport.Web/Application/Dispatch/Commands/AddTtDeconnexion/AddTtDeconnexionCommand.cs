using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddTtDeconnexion;

public record AddTtDeconnexionCommand(
    string NumeroTt,
    DateTime DateDebutUtc,
    string? Raison,
    bool RetireEffectif) : IRequest<Guid>;
