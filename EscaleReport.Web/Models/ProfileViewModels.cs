using System.ComponentModel.DataAnnotations;

namespace EscaleReport.Web.Models;

public class ProfileViewModel
{
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public IReadOnlyList<string> DispatchPosts { get; set; } = [];
    public IReadOnlyList<string> AvailableTeams { get; set; } = [];
    public IReadOnlyList<TeamHistoryViewModel> TeamHistory { get; set; } = [];

    public UpdateProfileViewModel UpdateProfile { get; set; } = new();
    public ChangePasswordViewModel ChangePassword { get; set; } = new();
}

// L'identifiant de connexion et le rôle restent hors de ce formulaire : le rôle conditionne
// les permissions (réservé à l'Administrateur, page Comptes & permissions) et l'identifiant
// est la clé stockée telle quelle dans BaseAuditableEntity.CreatedBy sur chaque enregistrement
// (anomalies, incidents, pannes...) ainsi que dans les filtres utilisateur/équipe des
// indicateurs (CDC §17) — le renommer romprait la lisibilité de l'historique déjà écrit.
public class UpdateProfileViewModel
{
    [Required(ErrorMessage = "E-mail requis.")]
    [EmailAddress(ErrorMessage = "Adresse e-mail invalide.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Poste par défaut")]
    public string? PosteParDefaut { get; set; }

    [Display(Name = "Équipe")]
    public string? Equipe { get; set; }
}

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Mot de passe actuel requis.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe actuel")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nouveau mot de passe requis.")]
    [DataType(DataType.Password)]
    [Display(Name = "Nouveau mot de passe")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmation requise.")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Les mots de passe ne correspondent pas.")]
    [Display(Name = "Confirmer le nouveau mot de passe")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
