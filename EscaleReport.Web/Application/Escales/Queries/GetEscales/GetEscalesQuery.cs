using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Application.Escales.Dtos;
using MediatR;

namespace EscaleReport.Web.Application.Escales.Queries.GetEscales;

public record GetEscalesQuery(
    int Page = 1,
    int PageSize = Paging.DefaultPageSize,
    string? Search = null,
    bool Terminees = false) : IRequest<PagedResult<EscaleDto>>;
