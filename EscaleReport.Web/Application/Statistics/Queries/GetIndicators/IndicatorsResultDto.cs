namespace EscaleReport.Web.Application.Statistics.Queries.GetIndicators;

public class IndicatorsResultDto
{
    // Filtres disponibles.
    public IReadOnlyList<string> NaviresDisponibles { get; set; } = [];
    public IReadOnlyList<string> LignesDisponibles { get; set; } = [];
    public IReadOnlyList<string> QuaisDisponibles { get; set; } = [];
    public IReadOnlyList<string> ShiftsDisponibles { get; set; } = [];
    public IReadOnlyList<string> CategoriesIncidentDisponibles { get; set; } = [];
    public IReadOnlyList<string> EquipementsDisponibles { get; set; } = [];
    public IReadOnlyList<string> UtilisateursDisponibles { get; set; } = [];
    public IReadOnlyList<string> EquipesDisponibles { get; set; } = [];

    // Escales.
    public int NombreEscales { get; set; }
    public int NombreEscalesTerminees { get; set; }
    public int NombreEscalesAvecAnomalies { get; set; }

    // Anomalies.
    public int NombreConteneursEnAnomalie { get; set; }
    public double? DelaiMoyenResolutionAnomaliesHeures { get; set; }

    // Incidents.
    public IReadOnlyList<(string Categorie, int Nombre)> IncidentsParCategorie { get; set; } = [];
    public int NombreIncidentsCritiques { get; set; }
    public double DureeTotaleIncidentsHeures { get; set; }

    // Équipements.
    public double DisponibiliteStsPct { get; set; }
    public double DisponibiliteTtPct { get; set; }
    public double DisponibiliteRtgPct { get; set; }
    public double DisponibiliteAutresEnginsPct { get; set; }
    public int NombrePannes { get; set; }
    public double DureeTotalePannesHeures { get; set; }
    public double TauxAffectationEquipementsPct { get; set; }

    // Conteneurs vides / additionnels / dangereux.
    public int ConteneursVidesSouhaites { get; set; }
    public int ConteneursVidesEmbarques { get; set; }
    public int ConteneursVidesCoupes { get; set; }
    public int ConteneursVidesRestants { get; set; }
    public int NombreConteneursAdditionnels { get; set; }
    public int NombreConteneursDangereux { get; set; }

    // Cargo.
    public double TauxRealisationDischPct { get; set; }
    public double TauxRealisationLoadPct { get; set; }

    // Rapports.
    public double? DelaiMoyenProductionRapportsHeures { get; set; }
    public int NombreRapportsEnvoyes { get; set; }

    // ITT.
    public int NombreTransfertsItt { get; set; }
    public double TauxAvancementTransfertsPct { get; set; }

    // Yard.
    public int NombreTachesYardRealisees { get; set; }
}
