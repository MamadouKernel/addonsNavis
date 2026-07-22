using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddAdditionalContainer;

public record AddAdditionalContainerCommand(
    Guid EscaleId,
    string NumeroConteneur,
    string? LigneMaritime,
    string? Position,
    Sens Sens,
    string? Commentaire,
    string? ReferenceEmail) : IRequest<Guid>;
