using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Cargo;

// CDC §10 : consolidation Cargo par escale — un enregistrement par escale (pas une liste),
// confrontant les volumes Disch/Load aux données EDI/TOS. Pattern différent des précédents :
// formulaire d'édition d'un agrégat unique avec indicateurs d'alerte dérivés, pas une
// ressource pool ni une liste d'enregistrements enfants.
public class CargoConsommation : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }

    // Disch (débarquement)
    public int DischHazard { get; set; }
    public int DischReefer { get; set; }
    public int DischOog { get; set; }
    public int DischImport { get; set; }
    public int DischRestow { get; set; }
    public int DischTranshipment { get; set; }
    public int Disch20Pieds { get; set; }
    public int Disch40Pieds { get; set; }
    public bool DischRealisee { get; set; }

    public int DischTotal => DischImport + DischRestow + DischTranshipment;

    // Load (embarquement)
    public int LoadYard { get; set; }
    public int LoadEnCommunication { get; set; }
    public bool LoadRealisee { get; set; }

    public int LoadTotal => LoadYard + LoadEnCommunication;

    // Revised Load (CDC §10.6)
    public bool RevisedLoadRecu { get; set; }
    public DateTime? RevisedLoadDateUtc { get; set; }
    public string? RevisedLoadPar { get; set; }
    public string? RevisedLoadObservations { get; set; }

    public void MarquerRevisedLoadRecu(string? par, string? observations)
    {
        RevisedLoadRecu = true;
        RevisedLoadDateUtc = DateTime.UtcNow;
        RevisedLoadPar = par;
        RevisedLoadObservations = observations;
    }

    // CDC §10.8 "Alertes Cargo" — dérivées, jamais ressaisies.
    public bool AlerteDischNonRenseigne => !DischRealisee;
    public bool AlerteLoadNonRenseigne => !LoadRealisee;
    public bool AlerteRevisedNonRecu => !RevisedLoadRecu;
}
