using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateStsIncident;

public record UpdateStsIncidentCommand(
    Guid IncidentId,
    Guid EscaleId,
    Guid? GantryId,
    string TypeIncident,
    DateTime DateDebutUtc,
    DateTime? DateFinUtc,
    string? Cause,
    string? ConditionsReprise,
    bool RetirePortiqueEffectif) : IRequest;
