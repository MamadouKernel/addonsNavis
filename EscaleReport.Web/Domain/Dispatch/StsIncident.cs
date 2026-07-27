using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Dispatch;

// CDC §6.3 "Incidents STS". TypeIncident = valeur paramétrable (ReferenceListKeys.StsIncidentType).
public class StsIncident : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }
    public Guid? GantryId { get; set; }

    public string TypeIncident { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public string? Cause { get; set; }
    public string? ConditionsReprise { get; set; }
    public bool RetirePortiqueEffectif { get; set; }

    public TimeSpan? Duree => DateFinUtc.HasValue ? DateFinUtc.Value - DateDebutUtc : null;
    public bool EstResolu => DateFinUtc.HasValue;

    public void MettreAJour(
        Guid escaleId,
        Guid? gantryId,
        string typeIncident,
        DateTime dateDebutUtc,
        DateTime? dateFinUtc,
        string? cause,
        string? conditionsReprise,
        bool retirePortiqueEffectif)
    {
        EscaleId = escaleId;
        GantryId = gantryId;
        TypeIncident = typeIncident.Trim();
        DateDebutUtc = dateDebutUtc;
        DateFinUtc = dateFinUtc.HasValue && dateFinUtc.Value >= dateDebutUtc ? dateFinUtc : null;
        Cause = cause;
        ConditionsReprise = conditionsReprise;
        RetirePortiqueEffectif = retirePortiqueEffectif;
    }

    public void Cloturer(string? conditionsReprise, DateTime? dateFinUtc = null)
    {
        var fin = dateFinUtc ?? DateTime.UtcNow;
        DateFinUtc = fin < DateDebutUtc ? DateDebutUtc : fin;
        if (!string.IsNullOrWhiteSpace(conditionsReprise))
        {
            ConditionsReprise = conditionsReprise;
        }
    }
}
