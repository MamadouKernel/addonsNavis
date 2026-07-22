using MediatR;

namespace EscaleReport.Web.Application.Escales.Commands.CreateEscale;

// Les 4 champs obligatoires "à terme" (CDC §4.2) restent nullable ici : le CDC autorise
// l'enregistrement provisoire en brouillon tant qu'ils ne sont pas tous renseignés
// (voir Escale.RefreshDraftState). Seul Navire est requis pour identifier la fiche.
public record CreateEscaleCommand(
    string Navire,
    string? Voyage,
    string? LigneMaritime,
    DateTime? Eta,
    string? VesselVisit,
    string? Quai,
    string? Shift,
    string? Planificateur) : IRequest<Guid>;
