using System.ComponentModel.DataAnnotations;

namespace EscaleReport.Web.Models;

public class ProfileViewModel
{
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? PosteParDefaut { get; set; }
    public string? Equipe { get; set; }

    public ChangePasswordViewModel ChangePassword { get; set; } = new();
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
