using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddStsIncident;

public record AddStsIncidentCommand(
    Guid EscaleId,
    Guid? GantryId,
    string TypeIncident,
    DateTime DateDebutUtc,
    string? Cause,
    bool RetirePortiqueEffectif) : IRequest<Guid>;
