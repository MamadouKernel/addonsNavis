using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddContainerAnomaly;

public record AddContainerAnomalyCommand(
    Guid EscaleId,
    string NumeroConteneur,
    Sens Sens,
    string? LigneMaritime,
    string? Position,
    string Raison,
    string? Commentaire,
    string? ReferenceEchange) : IRequest<Guid>;
