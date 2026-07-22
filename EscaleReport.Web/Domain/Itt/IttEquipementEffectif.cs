using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Itt;

// CDC §13.3 "Gestion des équipements ITT" — enregistrement "courant" unique, même principe
// que RtgEffectif/TtEffectif.
public class IttEquipementEffectif : BaseAuditableEntity
{
    public int Disponible { get; set; }
    public int Engage { get; set; }
    public int EnPanne { get; set; }
    public string? Observations { get; set; }
}
