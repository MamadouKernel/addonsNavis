using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateRtgEffectif;

public record UpdateRtgEffectifCommand(
    int TotalParc,
    int Disponible,
    int Affecte,
    int EnPanne,
    int Retire) : IRequest;
