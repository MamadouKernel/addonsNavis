using EscaleReport.Web.Application.Cargo.Dtos;
using EscaleReport.Web.Application.Common.Models;
using MediatR;

namespace EscaleReport.Web.Application.Cargo.Queries.GetCargoDashboard;

public record GetCargoDashboardQuery(int Page = 1, int PageSize = Paging.DefaultPageSize) : IRequest<PagedResult<CargoDashboardRowDto>>;
