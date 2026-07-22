using EscaleReport.Web.Application.Escales.Dtos;
using MediatR;

namespace EscaleReport.Web.Application.Escales.Queries.GetEscales;

public record GetEscalesQuery : IRequest<IReadOnlyList<EscaleDto>>;
