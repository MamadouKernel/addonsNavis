namespace EscaleReport.Web.Domain.Audit;

// Journal d'audit (CDC §2 "historique de ses actions" + permission "Consulter le journal
// d'audit", §18 traçabilité des actions). Alimenté automatiquement par
// AuditLoggingBehaviour pour chaque commande exécutée avec succès — pas de saisie manuelle,
// pas de dépendance à ASP.NET Core Identity ici (le lien utilisateur reste par Id/Nom bruts,
// capturés au moment de l'action pour rester exacts même si le compte est renommé ensuite).
public class AuditLogEntry
{
    public Guid Id { get; set; }
    public DateTime DateUtc { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Cible { get; set; }
    public string? EntityType { get; set; }
    public string? EntityId { get; set; }
    public string? ChangesJson { get; set; }
}
