using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateAutresEnginsEffectif;

public record UpdateAutresEnginsEffectifCommand(
    int DisponibleReachStackers,
    int DisponibleEmptyHandlers,
    int DisponibleAutres) : IRequest;
