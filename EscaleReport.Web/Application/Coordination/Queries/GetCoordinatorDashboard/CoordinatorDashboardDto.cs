using EscaleReport.Web.Domain.Escales;

namespace EscaleReport.Web.Application.Coordination.Queries.GetCoordinatorDashboard;

public class CoordinatorDashboardDto
{
    public IReadOnlyList<NavireSyntheseDto> SyntheseNavires { get; set; } = [];
    public StsSyntheseDto SyntheseSts { get; set; } = new();
    public TtSyntheseDto SyntheseTt { get; set; } = new();
    public RtgAutresEnginsSyntheseDto SyntheseRtgAutresEngins { get; set; } = new();
    public IReadOnlyList<CargoSyntheseDto> SyntheseCargo { get; set; } = [];
    public YardSyntheseDto SyntheseYard { get; set; } = new();
    public IReadOnlyList<CoordinatorIncidentDto> Incidents { get; set; } = [];
}

// CDC §12 "Synthèse navires" — PointsAttention est une valeur dérivée (anomalies + incidents
// non résolus sur ce navire), faute d'un champ de texte libre dédié dans le CDC.
public class NavireSyntheseDto
{
    public Guid Id { get; set; }
    public string Navire { get; set; } = string.Empty;
    public string Voyage { get; set; } = string.Empty;
    public string LigneMaritime { get; set; } = string.Empty;
    public string? Quai { get; set; }
    public DateTime Eta { get; set; }
    public DateTime? Ata { get; set; }
    public DateTime? Etc { get; set; }
    public StatutOperations StatutOperations { get; set; }
    public StatutPlanification StatutPlanification { get; set; }
    public int PointsAttention { get; set; }
}

public class StsSyntheseDto
{
    public int PortiquesDisponibles { get; set; }
    public int PortiquesAffectes { get; set; }
    public int PortiquesEnPanne { get; set; }
    public int IncidentsEnCours { get; set; }
    public TimeSpan TempsIndisponibiliteCumule { get; set; }
    public IReadOnlyList<string> NaviresConcernes { get; set; } = [];
}

// Déconnexions TT (CDC §7.3) non encore suivies dans l'application — champ volontairement
// absent plutôt que d'afficher une valeur à zéro trompeuse.
public class TtSyntheseDto
{
    public int EffectifTotal { get; set; }
    public int EffectifDesigne { get; set; }
    public int EffectifDisponible { get; set; }
    public int EcartsAffectation { get; set; }
}

public class RtgAutresEnginsSyntheseDto
{
    public int RtgDisponible { get; set; }
    public int RtgEnPanne { get; set; }
    public int RtgRetire { get; set; }
    public int EnginsDisponibles { get; set; }
    public int EnginsRetires { get; set; }
    public int Clashs { get; set; }
    public int ProblemesGate { get; set; }
    public int RemplacementsOperateurs { get; set; }
}

public class CargoSyntheseDto
{
    public string Navire { get; set; } = string.Empty;
    public bool DischFait { get; set; }
    public bool LoadFait { get; set; }
    public bool RevisedLoadRecu { get; set; }
    public bool RapportEnvoye { get; set; }
    public int AlertesOuvertes { get; set; }
}

public class YardSyntheseDto
{
    public int ZonesDebarquement { get; set; }
    public int TransfertsOutEnCours { get; set; }
    public int HousekeepingTotal { get; set; }
    public int TachesEnRetard { get; set; }
}

public class CoordinatorIncidentDto
{
    public Guid Id { get; set; }
    public string Objet { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string? Note { get; set; }
    public string? ActionRealisee { get; set; }
    public bool EstResolu { get; set; }
}
