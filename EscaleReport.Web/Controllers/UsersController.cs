using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Audit;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Infrastructure.Identity;
using EscaleReport.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Controllers;

// Administration des comptes et permissions (CDC §2 : "Un administrateur devra pouvoir créer,
// modifier, désactiver un utilisateur, réinitialiser son mot de passe, lui affecter un rôle/
// poste/équipe, lui attribuer ou retirer des permissions, consulter son historique d'activité").
// Accès direct à UserManager/RoleManager (comme AccountController) plutôt qu'un passage par
// MediatR : ce sont des opérations natives ASP.NET Core Identity, pas des règles métier Domain.
[Authorize(Roles = RoleAccessGroups.Administration)]
public class UsersController(
    UserManager<ApplicationUser> userManager,
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : Controller
{
    private static readonly string[] DispatchPosts = ["STS", "TT", "RTG", "Autres engins"];
    private const string TeamListKey = "Equipe";

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        EnsureAdmin();

        var users = await userManager.Users.OrderBy(u => u.UserName).ToListAsync(cancellationToken);
        var allPermissionRows = await dbContext.UserPermissions.AsNoTracking().ToListAsync(cancellationToken);
        var teams = await dbContext.ReferenceValues.AsNoTracking()
            .Where(r => r.ListKey == TeamListKey)
            .OrderByDescending(r => r.IsActive)
            .ThenBy(r => r.SortOrder)
            .ThenBy(r => r.Value)
            .ToListAsync(cancellationToken);
        var teamHistory = await dbContext.AuditLogEntries.AsNoTracking()
            .Where(a => a.Action.StartsWith("ChangeTeam:") && a.Cible != null)
            .OrderByDescending(a => a.DateUtc)
            .ToListAsync(cancellationToken);

        var rows = new List<UserRowViewModel>();
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            rows.Add(new UserRowViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email,
                Role = roles.FirstOrDefault(),
                PosteParDefaut = user.PosteParDefaut,
                Equipe = user.Equipe,
                IsActive = user.IsActive,
                IsLockedOut = user.LockoutEnd is not null && user.LockoutEnd > DateTimeOffset.UtcNow,
                Permissions = allPermissionRows.Where(p => p.UserId == user.Id).Select(p => p.PermissionKey).ToHashSet()
            });
        }

        foreach (var row in rows)
        {
            row.TeamHistory = teamHistory.Where(entry => entry.Cible == row.Id.ToString())
                .Select(ToTeamHistory).Take(10).ToList();
        }

        return View(new UsersIndexViewModel
        {
            Users = rows,
            Roles = Roles.All,
            AllPermissions = Permissions.All,
            DispatchPosts = DispatchPosts,
            Teams = teams.Select(team => new TeamRowViewModel
            {
                Id = team.Id,
                Name = team.Value,
                IsActive = team.IsActive,
                MemberCount = rows.Count(user => string.Equals(user.Equipe, team.Value, StringComparison.OrdinalIgnoreCase))
            }).ToList()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserInput input, CancellationToken cancellationToken)
    {
        EnsureAdmin();

        if (!ModelState.IsValid || !Roles.All.Contains(input.Role))
        {
            TempData["Error"] = "Formulaire invalide : vérifiez l'identifiant, le mot de passe et le rôle.";
            return RedirectToAction(nameof(Index));
        }

        var normalizedTeam = await GetActiveTeamAsync(input.Equipe, cancellationToken);
        if (!string.IsNullOrWhiteSpace(input.Equipe) && normalizedTeam is null)
        {
            TempData["Error"] = "L'équipe sélectionnée n'existe pas ou n'est plus active.";
            return RedirectToAction(nameof(Index));
        }

        var user = new ApplicationUser
        {
            UserName = input.UserName,
            Email = $"{input.UserName}@escalereport.local",
            EmailConfirmed = true,
            IsActive = true,
            PosteParDefaut = string.IsNullOrWhiteSpace(input.PosteParDefaut) ? null : input.PosteParDefaut,
            Equipe = normalizedTeam
        };

        var result = await userManager.CreateAsync(user, input.Password);
        if (!result.Succeeded)
        {
            TempData["Error"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Index));
        }

        await userManager.AddToRoleAsync(user, input.Role);
        await LogAuditAsync("CreateUser", user.Id.ToString(), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(Guid id, CancellationToken cancellationToken)
    {
        EnsureAdmin();
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is not null)
        {
            user.IsActive = !user.IsActive;
            await userManager.UpdateAsync(user);

            // Une désactivation ne doit pas laisser une session déjà ouverte utilisable : IsActive
            // n'est vérifié qu'à la connexion (AccountController.Login), donc sans ce coup de pouce
            // le cookie déjà émis resterait valide jusqu'à expiration. Changer le security stamp
            // invalide ce cookie dès la prochaine revalidation périodique (voir
            // SecurityStampValidatorOptions.ValidationInterval dans Program.cs).
            if (!user.IsActive)
            {
                await userManager.UpdateSecurityStampAsync(user);
            }

            await LogAuditAsync(user.IsActive ? "ActivateUser" : "DeactivateUser", id.ToString(), cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(Guid id, string newPassword, CancellationToken cancellationToken)
    {
        EnsureAdmin();
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
        {
            TempData["Error"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return RedirectToAction(nameof(Index));
        }

        await LogAuditAsync("ResetPassword", id.ToString(), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(Guid id, string role, CancellationToken cancellationToken)
    {
        EnsureAdmin();
        if (!Roles.All.Contains(role))
        {
            return RedirectToAction(nameof(Index));
        }

        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Count > 0)
        {
            await userManager.RemoveFromRolesAsync(user, currentRoles);
        }

        await userManager.AddToRoleAsync(user, role);
        await LogAuditAsync("ChangeRole:" + role, id.ToString(), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(Guid id, string? posteParDefaut, string? equipe, CancellationToken cancellationToken)
    {
        EnsureAdmin();
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is not null)
        {
            var previousTeam = user.Equipe;
            var normalizedTeam = string.Equals(user.Equipe, equipe?.Trim(), StringComparison.OrdinalIgnoreCase)
                ? user.Equipe
                : await GetActiveTeamAsync(equipe, cancellationToken);
            if (!string.IsNullOrWhiteSpace(equipe) && normalizedTeam is null)
            {
                TempData["Error"] = "L'équipe sélectionnée n'existe pas ou n'est plus active.";
                return RedirectToAction(nameof(Index));
            }
            user.PosteParDefaut = string.IsNullOrWhiteSpace(posteParDefaut) ? null : posteParDefaut;
            user.Equipe = normalizedTeam;
            await userManager.UpdateAsync(user);
            if (!string.Equals(previousTeam, user.Equipe, StringComparison.OrdinalIgnoreCase))
            {
                await LogAuditAsync($"ChangeTeam:{TeamLabel(previousTeam)}→{TeamLabel(user.Equipe)}", id.ToString(), cancellationToken);
            }
            await LogAuditAsync("UpdateUserProfile", id.ToString(), cancellationToken);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTeam(string name, CancellationToken cancellationToken)
    {
        EnsureAdmin();
        var normalized = name?.Trim() ?? string.Empty;
        if (normalized.Length is < 2 or > 80)
        {
            TempData["Error"] = "Le nom de l'équipe doit contenir entre 2 et 80 caractères.";
            return RedirectToAction(nameof(Index));
        }

        var exists = await dbContext.ReferenceValues.AnyAsync(
            r => r.ListKey == TeamListKey && r.Value.ToLower() == normalized.ToLower(), cancellationToken);
        if (exists)
        {
            TempData["Error"] = "Cette équipe existe déjà.";
            return RedirectToAction(nameof(Index));
        }

        var sortOrder = await dbContext.ReferenceValues.Where(r => r.ListKey == TeamListKey)
            .Select(r => (int?)r.SortOrder).MaxAsync(cancellationToken) ?? -1;
        dbContext.ReferenceValues.Add(new ReferenceValue
        {
            ListKey = TeamListKey,
            Value = normalized,
            SortOrder = sortOrder + 1,
            IsActive = true
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        await LogAuditAsync("CreateTeam:" + normalized, null, cancellationToken);
        TempData["Success"] = $"Équipe « {normalized} » créée.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleTeam(Guid id, CancellationToken cancellationToken)
    {
        EnsureAdmin();
        var team = await dbContext.ReferenceValues.FirstOrDefaultAsync(
            r => r.Id == id && r.ListKey == TeamListKey, cancellationToken);
        if (team is null) return RedirectToAction(nameof(Index));

        team.IsActive = !team.IsActive;
        await dbContext.SaveChangesAsync(cancellationToken);
        await LogAuditAsync((team.IsActive ? "ActivateTeam:" : "DeactivateTeam:") + team.Value, null, cancellationToken);
        TempData["Success"] = $"Équipe « {team.Value} » {(team.IsActive ? "activée" : "désactivée")}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePermission(Guid id, string permissionKey, CancellationToken cancellationToken)
    {
        EnsureAdmin();
        if (!Permissions.All.Contains(permissionKey))
        {
            return RedirectToAction(nameof(Index));
        }

        var existing = await dbContext.UserPermissions
            .FirstOrDefaultAsync(p => p.UserId == id && p.PermissionKey == permissionKey, cancellationToken);

        if (existing is not null)
        {
            dbContext.UserPermissions.Remove(existing);
        }
        else
        {
            dbContext.UserPermissions.Add(new UserPermission { UserId = id, PermissionKey = permissionKey });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await LogAuditAsync("TogglePermission:" + permissionKey, id.ToString(), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private void EnsureAdmin()
    {
        if (!currentUser.HasPermission(Permissions.AdministrerUtilisateurs))
        {
            throw new ForbiddenAccessException(Permissions.AdministrerUtilisateurs);
        }
    }

    private async Task<string?> GetActiveTeamAsync(string? requested, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(requested)) return null;
        var normalized = requested.Trim();
        return await dbContext.ReferenceValues.AsNoTracking()
            .Where(r => r.ListKey == TeamListKey && r.IsActive && r.Value.ToLower() == normalized.ToLower())
            .Select(r => r.Value)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static TeamHistoryViewModel ToTeamHistory(AuditLogEntry entry)
    {
        var transition = entry.Action["ChangeTeam:".Length..].Split('→', 2);
        return new TeamHistoryViewModel
        {
            DateUtc = entry.DateUtc,
            PreviousTeam = transition.ElementAtOrDefault(0),
            NewTeam = transition.ElementAtOrDefault(1),
            ChangedBy = entry.UserName ?? "Système"
        };
    }

    private static string TeamLabel(string? team) => string.IsNullOrWhiteSpace(team) ? "Sans équipe" : team;

    // Ces actions passent par UserManager/RoleManager en dehors du pipeline MediatR
    // (AuditLoggingBehaviour ne les voit donc pas) : la traçabilité est posée ici à la main,
    // ce qui importe particulièrement pour la gestion des comptes/permissions (CDC §2/§18).
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
