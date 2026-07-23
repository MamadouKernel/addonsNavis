using EscaleReport.Web.Application.Common.Models;
using MediatR;

namespace EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;

// Unbounded : utilisé par la génération PDF/Excel (CDC §14.3/§14.4), qui a besoin de la liste
// complète de chaque catégorie — un rapport imprimé ne peut pas se limiter à la page actuellement
// affichée à l'écran par un utilisateur au moment du clic sur "Générer".
public record GetEscaleDetailQuery(
    Guid EscaleId,
    int AnomaliesPage = 1,
    int VidesPage = 1,
    int IncidentsPage = 1,
    int AdditionnelsPage = 1,
    int DangereuxPage = 1,
    bool Unbounded = false) : IRequest<EscaleDetailDto?>;
