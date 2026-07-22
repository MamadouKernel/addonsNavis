using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddEmptyContainerTarget;

public record AddEmptyContainerTargetCommand(
    Guid EscaleId,
    string LigneMaritime,
    string TypeConteneur,
    int QuantiteSouhaitee) : IRequest<Guid>;
