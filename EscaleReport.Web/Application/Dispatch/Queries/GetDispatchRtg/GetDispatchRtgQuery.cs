using EscaleReport.Web.Application.Common.Models;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchRtg;

public record GetDispatchRtgQuery(
    int PannesPage = 1,
    int ClashesPage = 1,
    int GateTruckIssuesPage = 1,
    int StsIncidentsPage = 1) : IRequest<DispatchRtgDto>;
