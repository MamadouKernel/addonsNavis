using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.VesselPlanning;

// CDC §5.1 "Gestion des conteneurs en anomalie".
public class ContainerAnomaly : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }

    public string NumeroConteneur { get; set; } = string.Empty;
    public Sens Sens { get; set; }
    public string? LigneMaritime { get; set; }
    public string? Position { get; set; }

    // Valeur libre validée à la commande contre ReferenceValue(ListKey=AnomalyReason) —
    // volontairement pas un enum, cf. CDC §27 sur les listes administrables.
    public string Raison { get; set; } = string.Empty;

    public AnomalyStatus Statut { get; set; } = AnomalyStatus.NonResolu;
    public DateTime? DateResolutionUtc { get; set; }
    public string? ResoluPar { get; set; }
    public string? Commentaire { get; set; }
    public string? ReferenceEchange { get; set; }

    public void Resoudre(string? resoluPar)
    {
        Statut = AnomalyStatus.Resolu;
        DateResolutionUtc = DateTime.UtcNow;
        ResoluPar = resoluPar;
    }
}
