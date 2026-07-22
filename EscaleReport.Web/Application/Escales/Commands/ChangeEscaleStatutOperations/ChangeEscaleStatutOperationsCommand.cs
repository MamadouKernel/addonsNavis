using EscaleReport.Web.Domain.Escales;
using MediatR;

namespace EscaleReport.Web.Application.Escales.Commands.ChangeEscaleStatutOperations;

public record ChangeEscaleStatutOperationsCommand(Guid EscaleId, StatutOperations NouveauStatut) : IRequest;
