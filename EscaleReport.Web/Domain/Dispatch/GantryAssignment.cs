using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// Affectation d'un portique à un navire pour une fenêtre horaire (CDC §6.2) — c'est cette
// durée qui permettra de mesurer le temps réellement travaillé sur chaque navire.
public class GantryAssignment : BaseAuditableEntity
{
    public Guid GantryId { get; set; }
    public Guid EscaleId { get; set; }

    public DateTime HeureDebut { get; set; }
    public DateTime? HeureFin { get; set; }
    public string? TacheOuZone { get; set; }
    public AssignmentStatus Statut { get; set; } = AssignmentStatus.EnCours;

    public void Terminer()
    {
        Statut = AssignmentStatus.Terminee;
        HeureFin ??= DateTime.UtcNow;
    }
}
