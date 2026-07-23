namespace EscaleReport.Web.Application.Audit.Queries.GetAuditLog;

public class AuditLogResultDto
{
    public IReadOnlyList<AuditLogEntryDto> Entries { get; init; } = [];
    public IReadOnlyList<string> Utilisateurs { get; init; } = [];
    public IReadOnlyList<string> Actions { get; init; } = [];
    public int TotalCount { get; init; }
}

public class AuditLogEntryDto
{
    public Guid Id { get; init; }
    public DateTime DateUtc { get; init; }
    public string? UserName { get; init; }
    public string Action { get; init; } = string.Empty;
    public string? Cible { get; init; }
}
