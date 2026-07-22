using EscaleReport.Web.Domain.VesselPlanning;

namespace EscaleReport.Web.Application.VesselPlanning.Dtos;

public class DangerousContainerDto
{
    public Guid Id { get; set; }
    public string NumeroConteneur { get; set; } = string.Empty;
    public string? LigneMaritime { get; set; }
    public string? ClasseImo { get; set; }
    public string? Position { get; set; }
    public BadtStatus StatutBadt { get; set; }
    public DateTime? DateValiditeBadt { get; set; }
    public DangerousContainerStatus StatutOperationnel { get; set; }
    public string? Commentaire { get; set; }
    public bool AlerteRenouvellement { get; set; }

    public static DangerousContainerDto FromEntity(DangerousContainer c) => new()
    {
        Id = c.Id,
        NumeroConteneur = c.NumeroConteneur,
        LigneMaritime = c.LigneMaritime,
        ClasseImo = c.ClasseImo,
        Position = c.Position,
        StatutBadt = c.StatutBadt,
        DateValiditeBadt = c.DateValiditeBadt,
        StatutOperationnel = c.StatutOperationnel,
        Commentaire = c.Commentaire,
        AlerteRenouvellement = c.AlerteRenouvellement
    };
}
