using MediatR;

namespace EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;

public record GetEscaleDetailQuery(Guid EscaleId) : IRequest<EscaleDetailDto?>;
