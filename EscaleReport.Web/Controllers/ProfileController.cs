using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Audit;
using EscaleReport.Web.Infrastructure.Identity;
using EscaleReport.Web.Models;
using Microsoft.EntityFrameworkCore;
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
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : Controller
{
    private static readonly string[] DispatchPosts = ["STS", "TT", "RTG", "Autres engins"];

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound();
        }

        var roles = await userManager.GetRolesAsync(user);

        return View(await BuildViewModelAsync(user, roles.FirstOrDefault() ?? "", cancellationToken));
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
            var requestedTeam = string.IsNullOrWhiteSpace(model.Equipe) ? null : model.Equipe.Trim();
            var validTeam = requestedTeam is null || await dbContext.ReferenceValues.AsNoTracking()
                .AnyAsync(r => r.ListKey == "Equipe" && r.IsActive && r.Value == requestedTeam, cancellationToken);
            if (!validTeam)
            {
                ModelState.AddModelError("UpdateProfile.Equipe", "Cette équipe n'existe pas ou n'est plus active.");
            }
        }

        if (ModelState.IsValid)
        {
            var previousTeam = user.Equipe;
            user.PosteParDefaut = string.IsNullOrWhiteSpace(model.PosteParDefaut) ? null : model.PosteParDefaut;
            user.Equipe = string.IsNullOrWhiteSpace(model.Equipe) ? null : model.Equipe;

            var result = emailChanged
                ? await userManager.SetEmailAsync(user, model.Email)
                : await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                if (!string.Equals(previousTeam, user.Equipe, StringComparison.OrdinalIgnoreCase))
                {
                    await LogAuditAsync($"ChangeTeam:{TeamLabel(previousTeam)}→{TeamLabel(user.Equipe)}", user.Id.ToString(), cancellationToken);
                }
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
        var viewModel = await BuildViewModelAsync(user, roles.FirstOrDefault() ?? "", cancellationToken);
        viewModel.UpdateProfile = model;
        return View(nameof(Index), viewModel);
    }

    private async Task<ProfileViewModel> BuildViewModelAsync(ApplicationUser user, string role, CancellationToken cancellationToken)
    {
        var teams = await dbContext.ReferenceValues.AsNoTracking()
            .Where(r => r.ListKey == "Equipe" && r.IsActive).OrderBy(r => r.SortOrder).Select(r => r.Value)
            .ToListAsync(cancellationToken);
        var historyEntries = await dbContext.AuditLogEntries.AsNoTracking()
            .Where(a => a.Cible == user.Id.ToString() && a.Action.StartsWith("ChangeTeam:"))
            .OrderByDescending(a => a.DateUtc).Take(10).ToListAsync(cancellationToken);
        return new ProfileViewModel
        {
        UserName = user.UserName ?? "",
        Role = role,
        DispatchPosts = DispatchPosts,
        AvailableTeams = teams,
        TeamHistory = historyEntries.Select(entry =>
        {
            var transition = entry.Action["ChangeTeam:".Length..].Split('→', 2);
            return new TeamHistoryViewModel { DateUtc = entry.DateUtc, PreviousTeam = transition.ElementAtOrDefault(0), NewTeam = transition.ElementAtOrDefault(1), ChangedBy = entry.UserName ?? "Système" };
        }).ToList(),
        UpdateProfile = new UpdateProfileViewModel
        {
            Email = user.Email ?? "",
            PosteParDefaut = user.PosteParDefaut,
            Equipe = user.Equipe
        }
        };
    }

    private static string TeamLabel(string? team) => string.IsNullOrWhiteSpace(team) ? "Sans équipe" : team;

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
