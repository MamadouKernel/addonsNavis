using EscaleReport.Web.Domain.VesselPlanning;

namespace EscaleReport.Web.Application.VesselPlanning.Dtos;

public class ContainerAnomalyDto
{
    public Guid Id { get; set; }
    public string NumeroConteneur { get; set; } = string.Empty;
    public Sens Sens { get; set; }
    public string? LigneMaritime { get; set; }
    public string? Position { get; set; }
    public string Raison { get; set; } = string.Empty;
    public AnomalyStatus Statut { get; set; }
    public DateTime? DateResolutionUtc { get; set; }
    public string? ResoluPar { get; set; }
    public string? Commentaire { get; set; }

    public static ContainerAnomalyDto FromEntity(ContainerAnomaly a) => new()
    {
        Id = a.Id,
        NumeroConteneur = a.NumeroConteneur,
        Sens = a.Sens,
        LigneMaritime = a.LigneMaritime,
        Position = a.Position,
        Raison = a.Raison,
        Statut = a.Statut,
        DateResolutionUtc = a.DateResolutionUtc,
        ResoluPar = a.ResoluPar,
        Commentaire = a.Commentaire
    };
}
