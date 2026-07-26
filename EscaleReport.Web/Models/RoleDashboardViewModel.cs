namespace EscaleReport.Web.Models;

public sealed record RoleDashboardAction(
    string Title,
    string Description,
    string Controller,
    string Action,
    string Icon,
    string Priority = "normal");

public sealed class RoleDashboardViewModel
{
    public required string RoleLabel { get; init; }
    public required string Heading { get; init; }
    public required string Introduction { get; init; }
    public string? Poste { get; init; }
    public IReadOnlyList<RoleDashboardAction> PrimaryActions { get; init; } = [];
    public IReadOnlyList<RoleDashboardAction> OtherActions { get; init; } = [];
}
