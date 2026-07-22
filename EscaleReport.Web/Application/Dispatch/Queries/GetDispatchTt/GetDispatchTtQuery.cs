using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchTt;

public record GetDispatchTtQuery : IRequest<DispatchTtDto>;
