using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Queries.GetYardPlannerDashboard;

public record GetYardPlannerDashboardQuery : IRequest<YardPlannerDashboardDto>;
