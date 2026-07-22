using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Settings;

// CDC §15.1 "Paramètres généraux" — enregistrement "courant" unique, même principe que
// TtEffectif/RtgEffectif.
public class GeneralSettings : BaseAuditableEntity
{
    public string? NomSociete { get; set; }
    public string? PlanificateurParDefaut { get; set; }
}
