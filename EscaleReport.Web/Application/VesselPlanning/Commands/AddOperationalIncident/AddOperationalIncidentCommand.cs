using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddOperationalIncident;

public record AddOperationalIncidentCommand(
    Guid EscaleId,
    string Categorie,
    string? Localisation,
    DateTime DateDebutUtc,
    DateTime? DateFinUtc,
    IncidentGravite Gravite,
    string? Description) : IRequest<Guid>;
