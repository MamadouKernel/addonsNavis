namespace EscaleReport.Web.Domain.Dispatch;

// CDC §6.2 : statut courant d'un portique STS.
public enum GantryStatus
{
    Disponible = 0,
    Affecte = 1,
    EnPanne = 2,
    EnMaintenance = 3,
    Indisponible = 4
}
