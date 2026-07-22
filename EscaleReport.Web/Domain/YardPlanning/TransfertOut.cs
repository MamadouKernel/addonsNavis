using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.YardPlanning;

// CDC §11.3 "Transferts Out".
public class TransfertOut : BaseAuditableEntity
{
    public string Bay { get; set; } = string.Empty;
    public int NombreConteneurs { get; set; }
    public string? DetailOuDestination { get; set; }
    public DateTime HeureDebut { get; set; }
    public DateTime? HeureFin { get; set; }
    public string? Commentaire { get; set; }

    public bool EstTermine => HeureFin.HasValue;

    public void Terminer()
    {
        HeureFin = DateTime.UtcNow;
    }
}
