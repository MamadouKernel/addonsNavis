using EscaleReport.Web.Domain.Dispatch;

namespace EscaleReport.Web.Application.Dispatch.Dtos;

public class GantryDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public GantryStatus Statut { get; set; }

    public static GantryDto FromEntity(Gantry g) => new()
    {
        Id = g.Id,
        Code = g.Code,
        Statut = g.Statut
    };
}
