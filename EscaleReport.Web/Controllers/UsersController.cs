using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Audit;
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
[Authorize]
public class UsersController(
    UserManager<ApplicationUser> userManager,
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : Controller
{
    private static readonly string[] DispatchPosts = ["STS", "TT", "RTG", "Autres engins"];

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        EnsureAdmin();

        var users = await userManager.Users.OrderBy(u => u.UserName).ToListAsync(cancellationToken);
        var allPermissionRows = await dbContext.UserPermissions.AsNoTracking().ToListAsync(cancellationToken);

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

        return View(new UsersIndexViewModel
        {
            Users = rows,
            Roles = Roles.All,
            AllPermissions = Permissions.All,
            DispatchPosts = DispatchPosts
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

        var user = new ApplicationUser
        {
            UserName = input.UserName,
            Email = $"{input.UserName}@escalereport.local",
            EmailConfirmed = true,
            IsActive = true,
            PosteParDefaut = string.IsNullOrWhiteSpace(input.PosteParDefaut) ? null : input.PosteParDefaut,
            Equipe = string.IsNullOrWhiteSpace(input.Equipe) ? null : input.Equipe
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
            user.PosteParDefaut = string.IsNullOrWhiteSpace(posteParDefaut) ? null : posteParDefaut;
            user.Equipe = string.IsNullOrWhiteSpace(equipe) ? null : equipe;
            await userManager.UpdateAsync(user);
            await LogAuditAsync("UpdateUserProfile", id.ToString(), cancellationToken);
        }

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
