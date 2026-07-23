using EscaleReport.Web.Infrastructure.Identity;
using EscaleReport.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Libre-service : chaque utilisateur ne voit et ne modifie que son propre compte (aucun
// paramètre id pris depuis le client). La gestion des AUTRES comptes (rôle, poste, équipe,
// permissions) reste réservée à l'Administrateur via UsersController (CDC §2.2).
[Authorize]
public class ProfileController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound();
        }

        var roles = await userManager.GetRolesAsync(user);

        return View(new ProfileViewModel
        {
            UserName = user.UserName ?? "",
            Email = user.Email,
            Role = roles.FirstOrDefault() ?? "",
            PosteParDefaut = user.PosteParDefaut,
            Equipe = user.Equipe
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword([Bind(Prefix = "ChangePassword")] ChangePasswordViewModel model)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                // Le changement de mot de passe régénère le security stamp de l'utilisateur ;
                // sans ce rafraîchissement, le cookie de session en cours serait invalidé au
                // prochain contrôle et l'utilisateur serait déconnecté sans préavis.
                await signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "Mot de passe modifié avec succès.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        var roles = await userManager.GetRolesAsync(user);
        var viewModel = new ProfileViewModel
        {
            UserName = user.UserName ?? "",
            Email = user.Email,
            Role = roles.FirstOrDefault() ?? "",
            PosteParDefaut = user.PosteParDefaut,
            Equipe = user.Equipe,
            ChangePassword = model
        };
        return View(nameof(Index), viewModel);
    }
}
