using MediatR;

namespace EscaleReport.Web.Application.Statistics.Queries.GetIndicators;

public record GetIndicatorsQuery(
    DateOnly? DateDebut,
    DateOnly? DateFin,
    string? Shift,
    string? Navire,
    string? LigneMaritime,
    string? Quai,
    string? TypeIncident,
    string? Equipement,
    string? Utilisateur,
    string? Equipe) : IRequest<IndicatorsResultDto>;
