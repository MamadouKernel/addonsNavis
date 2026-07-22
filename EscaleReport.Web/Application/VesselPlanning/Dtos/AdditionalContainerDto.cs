using EscaleReport.Web.Domain.VesselPlanning;

namespace EscaleReport.Web.Application.VesselPlanning.Dtos;

public class AdditionalContainerDto
{
    public Guid Id { get; set; }
    public string NumeroConteneur { get; set; } = string.Empty;
    public string? LigneMaritime { get; set; }
    public string? Position { get; set; }
    public Sens Sens { get; set; }
    public AdditionalContainerDecision Decision { get; set; }
    public string? Commentaire { get; set; }
    public DateTime? DateDecisionUtc { get; set; }
    public string? DecidePar { get; set; }

    public static AdditionalContainerDto FromEntity(AdditionalContainer c) => new()
    {
        Id = c.Id,
        NumeroConteneur = c.NumeroConteneur,
        LigneMaritime = c.LigneMaritime,
        Position = c.Position,
        Sens = c.Sens,
        Decision = c.Decision,
        Commentaire = c.Commentaire,
        DateDecisionUtc = c.DateDecisionUtc,
        DecidePar = c.DecidePar
    };
}
