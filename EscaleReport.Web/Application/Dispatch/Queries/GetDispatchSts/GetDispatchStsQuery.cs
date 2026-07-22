using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchSts;

public record GetDispatchStsQuery : IRequest<DispatchStsDto>;
