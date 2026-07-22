using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Settings;

// CDC §15.1 "Les délais déclenchant les alertes" — magasin générique de seuils (en heures),
// administrable même si le moteur de notifications (§16) n'est pas encore construit.
public class AlertThreshold : BaseAuditableEntity
{
    public string Cle { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public int ValeurHeures { get; set; }
}
