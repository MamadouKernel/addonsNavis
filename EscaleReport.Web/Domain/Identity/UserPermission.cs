namespace EscaleReport.Web.Domain.Identity;

// Autorisation ponctuelle accordée à un utilisateur, indépendamment de son rôle (CDC §2.1).
// Absence de ligne pour une permission = non accordée (principe du moindre privilège).
// Pas de navigation vers ApplicationUser ici : cette entité reste dans Domain, sans
// dépendance à ASP.NET Core Identity (détail d'Infrastructure). Le lien FK est
// configuré côté Infrastructure (UserPermissionConfiguration).
public class UserPermission
{
    public Guid UserId { get; set; }
    public string PermissionKey { get; set; } = string.Empty;
}
