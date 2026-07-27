using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddDangerousContainer;

public record AddDangerousContainerCommand(
    Guid EscaleId,
    string NumeroConteneur,
    string? LigneMaritime,
    string? ClasseImo,
    string? Position,
    DateTime? DateValiditeBadt,
    BadtStatus StatutBadt = BadtStatus.NonPris,
    DangerousContainerStatus StatutOperationnel = DangerousContainerStatus.ASuivre) : IRequest<Guid>;
