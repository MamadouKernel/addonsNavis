using EscaleReport.Web.Application.Common.Models;
using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Queries.GetYardPlannerDashboard;

public record GetYardPlannerDashboardQuery(
    int NaviresPage = 1,
    int PlansPage = 1,
    int TransfertsPage = 1,
    int HousekeepingPage = 1) : IRequest<YardPlannerDashboardDto>;
