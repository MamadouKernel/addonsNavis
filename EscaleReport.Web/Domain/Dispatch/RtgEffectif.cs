using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §8.1 : effectif RTG du shift — enregistrement "courant" unique, même principe que
// TtEffectif et que le pool de portiques STS (pas d'historique par shift dans ce périmètre).
public class RtgEffectif : BaseAuditableEntity
{
    public int TotalParc { get; set; }
    public int Disponible { get; set; }
    public int Affecte { get; set; }
    public int EnPanne { get; set; }
    public int Retire { get; set; }
}
