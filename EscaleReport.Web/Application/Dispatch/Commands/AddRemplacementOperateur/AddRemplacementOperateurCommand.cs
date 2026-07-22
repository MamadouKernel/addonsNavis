using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddRemplacementOperateur;

public record AddRemplacementOperateurCommand(
    string Operateur,
    string EnginQuitte,
    string NouvelEngin,
    DateTime DateHeureUtc,
    string? Raison,
    string? Commentaire) : IRequest<Guid>;
