using EscaleReport.Web.Domain.Dispatch;

namespace EscaleReport.Web.Application.Dispatch.Dtos;

public class StsPointeurDto
{
    public Guid Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string? NavireOuZone { get; set; }
    public DateTime HeurePriseDePosteUtc { get; set; }
    public DateTime? HeureFinUtc { get; set; }
    public string? Remarque { get; set; }

    public static StsPointeurDto FromEntity(StsPointeur p) => new()
    {
        Id = p.Id,
        Nom = p.Nom,
        Role = p.Role,
        NavireOuZone = p.NavireOuZone,
        HeurePriseDePosteUtc = p.HeurePriseDePosteUtc,
        HeureFinUtc = p.HeureFinUtc,
        Remarque = p.Remarque
    };
}
