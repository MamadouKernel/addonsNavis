using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Audit;
using EscaleReport.Web.Infrastructure.Identity;
using EscaleReport.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EscaleReport.Web.Controllers;

public class AccountController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IApplicationDbContext dbContext,
    ILogger<AccountController> logger) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null) => View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await userManager.FindByNameAsync(model.UserName);
        if (user is null || !user.IsActive)
        {
            await LogFailedLoginAsync(user?.Id, model.UserName, "compte inconnu ou désactivé");
            // Message volontairement générique (CDC §2.4) : ne pas révéler si le compte existe.
            ModelState.AddModelError(string.Empty, "Identifiant ou mot de passe incorrect.");
            return View(model);
        }

        // CDC §2.4 : verrouillage après plusieurs échecs -> lockoutOnFailure: true.
        var result = await signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            await LogFailedLoginAsync(user.Id, model.UserName, "compte verrouillé");
            ModelState.AddModelError(string.Empty, "Compte verrouillé après plusieurs échecs. Réessayez plus tard.");
            return View(model);
        }

        if (!result.Succeeded)
        {
            await LogFailedLoginAsync(user.Id, model.UserName, "mot de passe incorrect");
            ModelState.AddModelError(string.Empty, "Identifiant ou mot de passe incorrect.");
            return View(model);
        }

        logger.LogInformation("Connexion réussie pour {UserName}", model.UserName);

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Escales");
    }

    // Journalise les tentatives de connexion refusées (CDC §2/§18 traçabilité) : ces événements
    // n'existaient auparavant que dans les logs applicatifs, jamais dans le journal d'audit
    // consultable par l'Administrateur.
    private async Task LogFailedLoginAsync(Guid? userId, string userName, string motif)
    {
        dbContext.AuditLogEntries.Add(new AuditLogEntry
        {
            Id = Guid.NewGuid(),
            DateUtc = DateTime.UtcNow,
            UserId = userId,
            UserName = userName,
            Action = $"Login (échec - {motif})"
        });
        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}
