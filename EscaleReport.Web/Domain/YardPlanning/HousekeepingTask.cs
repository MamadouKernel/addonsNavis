using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.YardPlanning;

public enum HousekeepingStatus { AFaire = 0, EnCours = 1, Termine = 2, Reporte = 3 }

// CDC §11.4 "Housekeeping" — le statut doit être tenu à jour en continu : c'est lui qui
// indique à la relève suivante où reprendre le travail engagé.
public class HousekeepingTask : BaseAuditableEntity
{
    public string Description { get; set; } = string.Empty;
    public string? Zone { get; set; }
    public HousekeepingStatus Statut { get; set; } = HousekeepingStatus.AFaire;
    public string? Priorite { get; set; }
    public string? Responsable { get; set; }
    public DateTime? DatePrevue { get; set; }
    public DateTime? DateRealisation { get; set; }
    public string? Note { get; set; }

    public void ChangerStatut(HousekeepingStatus statut)
    {
        Statut = statut;
        DateRealisation = statut == HousekeepingStatus.Termine ? DateTime.UtcNow : DateRealisation;
    }
}
