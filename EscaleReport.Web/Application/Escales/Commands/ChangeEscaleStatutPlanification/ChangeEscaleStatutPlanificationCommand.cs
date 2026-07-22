using EscaleReport.Web.Domain.Escales;
using MediatR;

namespace EscaleReport.Web.Application.Escales.Commands.ChangeEscaleStatutPlanification;

public record ChangeEscaleStatutPlanificationCommand(Guid EscaleId, StatutPlanification NouveauStatut) : IRequest;
