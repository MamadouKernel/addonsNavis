using EscaleReport.Web.Domain.VesselPlanning;

namespace EscaleReport.Web.Application.VesselPlanning.Dtos;

public class EmptyContainerTargetDto
{
    public Guid Id { get; set; }
    public string LigneMaritime { get; set; } = string.Empty;
    public string TypeConteneur { get; set; } = string.Empty;
    public int QuantiteSouhaitee { get; set; }
    public int QuantiteAjoutee { get; set; }
    public int QuantitePlanifiee { get; set; }
    public int QuantiteEmbarquee { get; set; }
    public int QuantiteCoupee { get; set; }
    public string? MotifCoupure { get; set; }
    public int QuantiteRestante { get; set; }

    public static EmptyContainerTargetDto FromEntity(EmptyContainerTarget t) => new()
    {
        Id = t.Id,
        LigneMaritime = t.LigneMaritime,
        TypeConteneur = t.TypeConteneur,
        QuantiteSouhaitee = t.QuantiteSouhaitee,
        QuantiteAjoutee = t.QuantiteAjoutee,
        QuantitePlanifiee = t.QuantitePlanifiee,
        QuantiteEmbarquee = t.QuantiteEmbarquee,
        QuantiteCoupee = t.QuantiteCoupee,
        MotifCoupure = t.MotifCoupure,
        QuantiteRestante = t.QuantiteRestante
    };
}
