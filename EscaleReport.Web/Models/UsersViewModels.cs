using System.ComponentModel.DataAnnotations;

namespace EscaleReport.Web.Models;

public class UsersIndexViewModel
{
    public IReadOnlyList<UserRowViewModel> Users { get; init; } = [];
    public IReadOnlyList<string> Roles { get; init; } = [];
    public IReadOnlyList<string> AllPermissions { get; init; } = [];
    public IReadOnlyList<string> DispatchPosts { get; init; } = [];
    public IReadOnlyList<TeamRowViewModel> Teams { get; init; } = [];
}

public class TeamRowViewModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public int MemberCount { get; init; }
}

public class TeamHistoryViewModel
{
    public DateTime DateUtc { get; init; }
    public string? PreviousTeam { get; init; }
    public string? NewTeam { get; init; }
    public string ChangedBy { get; init; } = string.Empty;
}

public class UserRowViewModel
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Matricule { get; init; }
    public string? NomComplet { get; init; }
    public string? Fonction { get; init; }
    public string? Service { get; init; }
    public string? Societe { get; init; }
    public string? Role { get; init; }
    public string? PosteParDefaut { get; init; }
    public string? Equipe { get; init; }
    public bool IsActive { get; init; }
    public bool IsLockedOut { get; init; }
    public IReadOnlySet<string> Permissions { get; init; } = new HashSet<string>();
    public IReadOnlyList<TeamHistoryViewModel> TeamHistory { get; set; } = [];
}

public class CreateUserInput
{
    [Required] public string Matricule { get; set; } = string.Empty;
    [Required] public string Nom { get; set; } = string.Empty;
    [Required] public string Prenoms { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    [Required] public string Fonction { get; set; } = string.Empty;
    [Required] public string Service { get; set; } = string.Empty;
    public string? Societe { get; set; }
    public string? SiteAffectation { get; set; }
    public string? ResponsableHierarchique { get; set; }
    public DateOnly? DateEntree { get; set; }
    public DateTime? DateExpirationCompteUtc { get; set; }
    [Required(ErrorMessage = "Identifiant requis.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mot de passe requis.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rôle requis.")]
    public string Role { get; set; } = string.Empty;

    public string? PosteParDefaut { get; set; }
    public string? Equipe { get; set; }
}
