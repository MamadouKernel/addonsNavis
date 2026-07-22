using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §9.4 "Remplacement des opérateurs" — journal des changements d'engin d'un opérateur ;
// pas de cycle ouvert/fermé, chaque enregistrement est un événement ponctuel.
public class RemplacementOperateur : BaseAuditableEntity
{
    public string Operateur { get; set; } = string.Empty;
    public string EnginQuitte { get; set; } = string.Empty;
    public string NouvelEngin { get; set; } = string.Empty;
    public DateTime DateHeureUtc { get; set; }
    public string? Raison { get; set; }
    public string? Commentaire { get; set; }
}
