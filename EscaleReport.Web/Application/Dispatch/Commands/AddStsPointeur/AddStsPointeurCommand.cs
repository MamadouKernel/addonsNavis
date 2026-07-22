using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddStsPointeur;

public record AddStsPointeurCommand(
    string Nom,
    string? Role,
    string? NavireOuZone,
    DateTime HeurePriseDePosteUtc) : IRequest<Guid>;
