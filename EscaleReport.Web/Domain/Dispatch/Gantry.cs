using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// Portique de quai STS (CDC §6.2). Pool fixe (CR1..CR8) — contrairement aux anomalies
// conteneurs (enregistrements ajoutés librement), un portique est une ressource physique
// dont on fait évoluer le statut, pas qu'on crée à la demande.
public class Gantry : BaseAuditableEntity
{
    public string Code { get; set; } = string.Empty; // ex. "CR1"
    public GantryStatus Statut { get; set; } = GantryStatus.Disponible;

    public void ChangerStatut(GantryStatus nouveauStatut) => Statut = nouveauStatut;
}
