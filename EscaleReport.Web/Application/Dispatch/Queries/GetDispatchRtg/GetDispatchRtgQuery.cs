using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchRtg;

public record GetDispatchRtgQuery : IRequest<DispatchRtgDto>;
