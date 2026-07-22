using EscaleReport.Web.Application.Cargo.Dtos;
using EscaleReport.Web.Application.Escales.Dtos;

namespace EscaleReport.Web.Application.Cargo.Queries.GetCargoDetail;

public class CargoDetailDto
{
    public EscaleDto Escale { get; set; } = null!;
    public CargoConsommationDto Consommation { get; set; } = new();
}
