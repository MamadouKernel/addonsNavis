using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchAutresEngins;

public record GetDispatchAutresEnginsQuery : IRequest<DispatchAutresEnginsDto>;
