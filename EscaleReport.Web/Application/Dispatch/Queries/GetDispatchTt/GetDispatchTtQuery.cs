using EscaleReport.Web.Application.Common.Models;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchTt;

public record GetDispatchTtQuery(int AssignmentsPage = 1, int DeconnexionsPage = 1) : IRequest<DispatchTtDto>;
