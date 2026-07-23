using EscaleReport.Web.Application.Common.Models;
using MediatR;

namespace EscaleReport.Web.Application.Itt.Queries.GetIttDashboard;

public record GetIttDashboardQuery(
    int TransfersPage = 1,
    int IncidentsPage = 1,
    int PannesPage = 1) : IRequest<IttDashboardDto>;
