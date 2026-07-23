using EscaleReport.Web.Application.Common.Models;
using MediatR;

namespace EscaleReport.Web.Application.Coordination.Queries.GetCoordinatorDashboard;

public record GetCoordinatorDashboardQuery(
    int NaviresPage = 1,
    int CargoPage = 1,
    int IncidentsPage = 1) : IRequest<CoordinatorDashboardDto>;
