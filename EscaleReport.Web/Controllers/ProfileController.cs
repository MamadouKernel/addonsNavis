using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Audit;
using EscaleReport.Web.Infrastructure.Identity;
using EscaleReport.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Libre-service : chaque utilisateur ne voit et ne modifie que son propre compte (aucun
// paramètre id pris depuis le client). Rôle et identifiant restent hors de ce contrôleur et
// ne sont modifiables que par un Administrateur via UsersController (CDC §2.2) — voir le
// commentaire de UpdateProfileViewModel pour la raison du verrouillage de l'identifiant.
[Authorize]
public class ProfileController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : Controller
{
    private static readonly string[] DispatchPosts = ["STS", "TT", "RTG", "Autres engins"];

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound();
        }

        var roles = await userManager.GetRolesAsync(user);

        return View(BuildViewModel(user, roles.FirstOrDefault() ?? ""));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(
        [Bind(Prefix = "UpdateProfile")] UpdateProfileViewModel model,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound();
        }

        var emailChanged = !string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase);
        if (ModelState.IsValid && emailChanged)
        {
            var existing = await userManager.FindByEmailAsync(model.Email);
            if (existing is not null && existing.Id != user.Id)
            {
                ModelState.AddModelError("UpdateProfile.Email", "Cette adresse e-mail est déjà utilisée par un autre compte.");
            }
        }

        if (ModelState.IsValid)
        {
            user.PosteParDefaut = string.IsNullOrWhiteSpace(model.PosteParDefaut) ? null : model.PosteParDefaut;
            user.Equipe = string.IsNullOrWhiteSpace(model.Equipe) ? null : model.Equipe;

            var result = emailChanged
                ? await userManager.SetEmailAsync(user, model.Email)
                : await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await LogAuditAsync("UpdateOwnProfile", user.Id.ToString(), cancellationToken);
                TempData["Success"] = "Profil mis à jour.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("UpdateProfile", error.Description);
            }
        }

        var roles = await userManager.GetRolesAsync(user);
        var viewModel = BuildViewModel(user, roles.FirstOrDefault() ?? "");
        viewModel.UpdateProfile = model;
        return View(nameof(Index), viewModel);
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
                ModelState.AddModelError("ChangePassword", error.Description);
            }
        }

        var roles = await userManager.GetRolesAsync(user);
        var viewModel = BuildViewModel(user, roles.FirstOrDefault() ?? "");
        viewModel.ChangePassword = model;
        return View(nameof(Index), viewModel);
    }

    private ProfileViewModel BuildViewModel(ApplicationUser user, string role) => new()
    {
        UserName = user.UserName ?? "",
        Role = role,
        DispatchPosts = DispatchPosts,
        UpdateProfile = new UpdateProfileViewModel
        {
            Email = user.Email ?? "",
            PosteParDefaut = user.PosteParDefaut,
            Equipe = user.Equipe
        }
    };

    // Ces actions passent par UserManager en dehors du pipeline MediatR (AuditLoggingBehaviour
    // ne les voit donc pas) : la traçabilité est posée ici à la main, comme dans UsersController.
    private async Task LogAuditAsync(string action, string? cible, CancellationToken cancellationToken)
    {
        dbContext.AuditLogEntries.Add(new AuditLogEntry
        {
            Id = Guid.NewGuid(),
            DateUtc = DateTime.UtcNow,
            UserId = currentUser.UserId,
            UserName = currentUser.UserName,
            Action = action,
            Cible = cible
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
