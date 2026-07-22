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
    public const string CutReason = "CutReason";
    public const string StsIncidentType = "StsIncidentType";
    public const string YardZone = "YardZone";
    public const string Shift = "Shift";
}
