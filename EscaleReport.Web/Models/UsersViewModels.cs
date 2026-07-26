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

public class UserRowViewModel
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Role { get; init; }
    public string? PosteParDefaut { get; init; }
    public string? Equipe { get; init; }
    public bool IsActive { get; init; }
    public bool IsLockedOut { get; init; }
    public IReadOnlySet<string> Permissions { get; init; } = new HashSet<string>();
}

public class CreateUserInput
{
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
