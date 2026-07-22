using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateTtEffectif;

public record UpdateTtEffectifCommand(
    int TotalParc,
    int Designes,
    string? RaisonNonDesignation,
    int Retires) : IRequest;
