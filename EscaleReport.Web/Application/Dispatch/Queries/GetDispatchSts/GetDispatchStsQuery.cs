using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchSts;

// CDC §6.1 "Sélection du shift" : Date/Shift optionnels — non fournis, la sélection reste
// "tous les shifts" (le Dispatcher n'est pas bloqué avant d'avoir choisi).
public record GetDispatchStsQuery(DateOnly? Date = null, string? Shift = null) : IRequest<DispatchStsDto>;
