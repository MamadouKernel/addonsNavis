using MediatR;

namespace EscaleReport.Web.Application.Settings.Queries.GetParametrage;

public record GetParametrageQuery : IRequest<ParametrageDto>;
