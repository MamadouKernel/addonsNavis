using EscaleReport.Web.Application.Cargo.Dtos;
using MediatR;

namespace EscaleReport.Web.Application.Cargo.Queries.GetCargoDashboard;

public record GetCargoDashboardQuery : IRequest<IReadOnlyList<CargoDashboardRowDto>>;
