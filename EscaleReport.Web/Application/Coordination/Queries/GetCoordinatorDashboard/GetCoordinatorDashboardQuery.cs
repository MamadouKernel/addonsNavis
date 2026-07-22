using MediatR;

namespace EscaleReport.Web.Application.Coordination.Queries.GetCoordinatorDashboard;

public record GetCoordinatorDashboardQuery : IRequest<CoordinatorDashboardDto>;
