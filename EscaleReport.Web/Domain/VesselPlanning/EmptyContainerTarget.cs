using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.VesselPlanning;

// CDC §5.2 "Gestion des conteneurs vides à embarquer".
public class EmptyContainerTarget : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }

    public string LigneMaritime { get; set; } = string.Empty;
    public string TypeConteneur { get; set; } = string.Empty;

    public int QuantiteSouhaitee { get; set; }
    public int QuantiteAjoutee { get; set; }
    public int QuantitePlanifiee { get; set; }
    public int QuantiteEmbarquee { get; set; }
    public int QuantiteCoupee { get; set; }
    public string? MotifCoupure { get; set; }

    // CDC : "Reste = quantité souhaitée + quantité ajoutée − quantité embarquée − quantité coupée"
    public int QuantiteRestante => Math.Max(0, QuantiteSouhaitee + QuantiteAjoutee - QuantiteEmbarquee - QuantiteCoupee);
}
