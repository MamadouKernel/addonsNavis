using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.UpdateEmptyContainerTarget;

public record UpdateEmptyContainerTargetCommand(
    Guid Id,
    Guid EscaleId,
    int QuantiteAjoutee,
    int QuantitePlanifiee,
    int QuantiteEmbarquee,
    int QuantiteCoupee,
    string? MotifCoupure) : IRequest;
