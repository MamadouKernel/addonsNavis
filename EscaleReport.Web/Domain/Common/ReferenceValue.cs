namespace EscaleReport.Web.Domain.Common;

// Liste de valeurs paramétrable (raisons d'anomalie, catégories d'incident, motifs de
// coupure...) — CDC §27 : "les valeurs de référence devront être administrables sans
// modification du code source". ListKey identifie la liste (voir ReferenceListKeys),
// Value est la valeur affichée/stockée sur les entités métier (ex. ContainerAnomaly.Raison).
public class ReferenceValue
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ListKey { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public static class ReferenceListKeys
{
    public const string AnomalyReason = "AnomalyReason";
    public const string IncidentCategory = "IncidentCategory";
    public const string IncidentSeverity = "IncidentSeverity";
    public const string CutReason = "CutReason";
    public const string StsIncidentType = "StsIncidentType";
    public const string StsVesselIncidentType = "StsVesselIncidentType";
    public const string YardZone = "YardZone";
    public const string Shift = "Shift";
    public const string Quai = "Quai";
    public const string LigneMaritime = "LigneMaritime";
    public const string Bay = "Bay";

    // CDC §15.1 : libellés affichés dans l'écran de paramétrage, associés à chaque clé.
    public static readonly IReadOnlyDictionary<string, string> Labels = new Dictionary<string, string>
    {
        [AnomalyReason] = "Raisons d'anomalie",
        [IncidentCategory] = "Catégories d'incident",
        [IncidentSeverity] = "Gravités d'incident",
        [CutReason] = "Motifs de coupure",
        [StsIncidentType] = "Types de panne portique STS",
        [StsVesselIncidentType] = "Types d'incident navire STS",
        [YardZone] = "Zones Yard",
        [Shift] = "Shifts",
        [Quai] = "Quais",
        [LigneMaritime] = "Lignes maritimes",
        [Bay] = "Bays"
    };
}
