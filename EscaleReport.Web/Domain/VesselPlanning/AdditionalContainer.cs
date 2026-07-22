using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.VesselPlanning;

public enum AdditionalContainerDecision { EnAttente = 0, Debarque = 1, Embarque = 2, Refuse = 3, Reporte = 4 }

// CDC §5.4 "Gestion des conteneurs additionnels".
public class AdditionalContainer : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }

    public string NumeroConteneur { get; set; } = string.Empty;
    public string? LigneMaritime { get; set; }
    public string? Position { get; set; }
    public Sens Sens { get; set; }
    public AdditionalContainerDecision Decision { get; set; } = AdditionalContainerDecision.EnAttente;
    public string? Commentaire { get; set; }
    public string? ReferenceEmail { get; set; }
    public DateTime? DateDecisionUtc { get; set; }
    public string? DecidePar { get; set; }

    public void EnregistrerDecision(AdditionalContainerDecision decision, string? decidePar)
    {
        Decision = decision;
        DateDecisionUtc = DateTime.UtcNow;
        DecidePar = decidePar;
    }
}
