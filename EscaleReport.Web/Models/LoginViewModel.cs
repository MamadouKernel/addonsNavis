using System.ComponentModel.DataAnnotations;

namespace EscaleReport.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Identifiant requis.")]
    [Display(Name = "Identifiant")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mot de passe requis.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe")]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
