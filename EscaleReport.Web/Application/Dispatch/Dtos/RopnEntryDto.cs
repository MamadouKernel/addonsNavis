using EscaleReport.Web.Domain.Dispatch;

namespace EscaleReport.Web.Application.Dispatch.Dtos;

public class RopnEntryDto
{
    public Guid Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string DifficulteRencontree { get; set; } = string.Empty;
    public string? ActionRealisee { get; set; }
    public RopnStatus Statut { get; set; }
    public string? Commentaire { get; set; }

    public static RopnEntryDto FromEntity(RopnEntry r) => new()
    {
        Id = r.Id,
        Nom = r.Nom,
        Role = r.Role,
        DifficulteRencontree = r.DifficulteRencontree,
        ActionRealisee = r.ActionRealisee,
        Statut = r.Statut,
        Commentaire = r.Commentaire
    };
}
