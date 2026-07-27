using MediatR;

namespace EscaleReport.Web.Application.Escales.Commands.UpdateEscale;

public record UpdateEscaleCommand(
    Guid Id,
    string Navire,
    string? Voyage,
    string? LigneMaritime,
    DateTime? Eta,
    DateTime? Ata,
    DateTime? Etc,
    string? VesselVisit,
    string? Quai,
    string? Shift,
    string? Planificateur) : IRequest;
