using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Itt;

// CDC §13.1 "Suivi des transferts" — par navire de connexion, distinct des escales suivies
// par le module Escales (le navire de connexion n'appelle pas nécessairement au terminal).
public class IttTransfer : BaseAuditableEntity
{
    public string NavireConnexion { get; set; } = string.Empty;
    public int NombreATransferer { get; set; }
    public int NombreTransfere { get; set; }
    public int NombreRecu { get; set; }
    public string? Observations { get; set; }

    public int NombreRestant => Math.Max(0, NombreATransferer - NombreTransfere);

    public double PourcentageAvancement =>
        NombreATransferer <= 0 ? 0 : Math.Round(100.0 * NombreTransfere / NombreATransferer, 1);
}
