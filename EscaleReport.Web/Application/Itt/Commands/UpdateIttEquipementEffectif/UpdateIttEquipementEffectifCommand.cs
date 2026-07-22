using MediatR;

namespace EscaleReport.Web.Application.Itt.Commands.UpdateIttEquipementEffectif;

public record UpdateIttEquipementEffectifCommand(
    int Disponible,
    int Engage,
    int EnPanne,
    string? Observations) : IRequest;
