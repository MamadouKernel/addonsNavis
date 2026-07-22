using MediatR;

namespace EscaleReport.Web.Application.Itt.Queries.GetIttDashboard;

public record GetIttDashboardQuery : IRequest<IttDashboardDto>;
