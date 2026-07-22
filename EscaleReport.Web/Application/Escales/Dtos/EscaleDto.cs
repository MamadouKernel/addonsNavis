using EscaleReport.Web.Domain.Escales;

namespace EscaleReport.Web.Application.Escales.Dtos;

public class EscaleDto
{
    public Guid Id { get; set; }
    public string Navire { get; set; } = string.Empty;
    public string Voyage { get; set; } = string.Empty;
    public string LigneMaritime { get; set; } = string.Empty;
    public string? VesselVisit { get; set; }
    public string? Quai { get; set; }
    public DateTime Eta { get; set; }
    public DateTime? Ata { get; set; }
    public DateTime? Etc { get; set; }
    public StatutOperations StatutOperations { get; set; }
    public StatutPlanification StatutPlanification { get; set; }
    public bool IsDraft { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public string? UpdatedBy { get; set; }

    public static EscaleDto FromEntity(Escale e) => new()
    {
        Id = e.Id,
        Navire = e.Navire,
        Voyage = e.Voyage,
        LigneMaritime = e.LigneMaritime,
        VesselVisit = e.VesselVisit,
        Quai = e.Quai,
        Eta = e.Eta,
        Ata = e.Ata,
        Etc = e.Etc,
        StatutOperations = e.StatutOperations,
        StatutPlanification = e.StatutPlanification,
        IsDraft = e.IsDraft,
        UpdatedAtUtc = e.UpdatedAtUtc,
        UpdatedBy = e.UpdatedBy
    };
}
