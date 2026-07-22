using Microsoft.AspNetCore.Identity;

namespace EscaleReport.Web.Infrastructure.Identity;

// Le "rôle principal" (Administrateur, Vessel Planner, Dispatcher, ...) est porté par
// ASP.NET Core Identity Roles. Les permissions restent décorrélées du rôle (CDC §2.1) :
// voir UserPermission, qui autorise un ajustement fin sans créer un rôle par cas particulier.
public class ApplicationUser : IdentityUser<Guid>
{
    // Poste par défaut proposé à la connexion pour un Dispatcher (STS/TT/RTG/Autres engins).
    // Le CDC précise que le poste du jour ne vaut que pour la session en cours (§2.2) :
    // ce champ n'est qu'une valeur de confort, pas la source de vérité de la session active.
    public string? PosteParDefaut { get; set; }

    public string? Equipe { get; set; }

    public bool IsActive { get; set; } = true;
}
