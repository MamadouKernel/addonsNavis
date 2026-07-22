using EscaleReport.Web.Domain.Dispatch;

namespace EscaleReport.Web.Application.Dispatch.Dtos;

public class GantryAssignmentDto
{
    public Guid Id { get; set; }
    public Guid GantryId { get; set; }
    public string GantryCode { get; set; } = string.Empty;
    public Guid EscaleId { get; set; }
    public string Navire { get; set; } = string.Empty;
    public DateTime HeureDebut { get; set; }
    public DateTime? HeureFin { get; set; }
    public string? TacheOuZone { get; set; }
    public AssignmentStatus Statut { get; set; }
}
