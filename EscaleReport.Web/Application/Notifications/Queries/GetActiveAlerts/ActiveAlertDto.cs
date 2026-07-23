namespace EscaleReport.Web.Application.Notifications.Queries.GetActiveAlerts;

public enum AlertSeverite { Info = 0, Avertissement = 1, Critique = 2 }

public class ActiveAlertDto
{
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public AlertSeverite Severite { get; set; } = AlertSeverite.Avertissement;
    public Guid? EscaleId { get; set; }
    public string? Navire { get; set; }
}

public class ActiveAlertsResultDto
{
    public IReadOnlyList<ActiveAlertDto> Alertes { get; set; } = [];
    public int NombreCritiques { get; set; }
}
