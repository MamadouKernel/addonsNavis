namespace EscaleReport.Web.Application.Dispatch.Dtos;

public class StsIncidentDto
{
    public Guid Id { get; set; }
    public Guid EscaleId { get; set; }
    public Guid? GantryId { get; set; }
    public string Navire { get; set; } = string.Empty;
    public string? GantryCode { get; set; }
    public string TypeIncident { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string? Cause { get; set; }
    public string? ConditionsReprise { get; set; }
    public bool RetirePortiqueEffectif { get; set; }
    public bool EstResolu { get; set; }
}
