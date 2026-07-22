using MediatR;

namespace EscaleReport.Web.Application.Cargo.Queries.GetCargoDetail;

public record GetCargoDetailQuery(Guid EscaleId) : IRequest<CargoDetailDto?>;
