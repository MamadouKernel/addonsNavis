using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
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
    IAuthenticationEmailSender emailSender,
    IConfiguration configuration,
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

        var requireItAdminMfa = configuration.GetValue("Security:RequireItAdminMfa", true);
        if (requireItAdminMfa && await userManager.IsInRoleAsync(user, Domain.Identity.Roles.AdministrateurIT))
        {
            if (string.IsNullOrWhiteSpace(user.Email) || !user.EmailConfirmed)
            {
                logger.LogWarning("Connexion IT refusée : adresse MFA absente ou non confirmée pour {UserName}", model.UserName);
                ModelState.AddModelError(string.Empty, "Le compte IT n'a pas d'adresse MFA valide. Contactez l'équipe IT.");
                return View(model);
            }
            if (!user.TwoFactorEnabled)
            {
                user.TwoFactorEnabled = true;
                await userManager.UpdateAsync(user);
            }
        }

        // CDC §2.4 : verrouillage après plusieurs échecs -> lockoutOnFailure: true.
        var result = await signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            await LogFailedLoginAsync(user.Id, model.UserName, "compte verrouillé");
            ModelState.AddModelError(string.Empty, "Compte verrouillé après plusieurs échecs. Réessayez plus tard.");
            return View(model);
        }

        if (result.RequiresTwoFactor)
        {
            try
            {
                await SendMfaCodeAsync(user, HttpContext.RequestAborted);
                return RedirectToAction(nameof(VerifyMfa), new { model.ReturnUrl });
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Impossible d'envoyer le code MFA pour {UserName}", model.UserName);
                await signInManager.SignOutAsync();
                ModelState.AddModelError(string.Empty, "Le code de sécurité n'a pas pu être envoyé. Contactez l'équipe IT.");
                return View(model);
            }
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

    [HttpGet]
    [Authorize(Roles = Roles.Dispatcher)]
    public async Task<IActionResult> ChooseDispatchPost()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound();
        }

        ViewBag.CurrentPost = user.PosteParDefaut;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = Roles.Dispatcher)]
    public async Task<IActionResult> ChooseDispatchPost(string poste)
    {
        string[] allowedPosts = ["STS", "TT", "RTG", "Autres engins"];
        if (!allowedPosts.Contains(poste, StringComparer.Ordinal))
        {
            ModelState.AddModelError(string.Empty, "Sélectionnez un poste valide.");
            return await ChooseDispatchPost();
        }

        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound();
        }

        user.PosteParDefaut = poste;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Impossible d'enregistrer le poste du jour.");
            return await ChooseDispatchPost();
        }

        await signInManager.RefreshSignInAsync(user);

        var action = poste switch
        {
            "TT" => "Tt",
            "RTG" => "Rtg",
            "Autres engins" => "AutresEngins",
            _ => "Sts"
        };
        return RedirectToAction(action, "Dispatch");
    }

    [HttpGet]
    public async Task<IActionResult> VerifyMfa(string? returnUrl = null)
    {
        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null) return RedirectToAction(nameof(Login));
        return View(new VerifyMfaViewModel { ReturnUrl = returnUrl, MaskedEmail = MaskEmail(user.Email) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("mfa")]
    public async Task<IActionResult> VerifyMfa(VerifyMfaViewModel model)
    {
        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null) return RedirectToAction(nameof(Login));
        model.MaskedEmail = MaskEmail(user.Email);
        if (!ModelState.IsValid) return View(model);

        var result = await signInManager.TwoFactorSignInAsync(
            TokenOptions.DefaultEmailProvider, model.Code.Replace(" ", string.Empty),
            isPersistent: false, rememberClient: false);
        if (result.IsLockedOut)
        {
            await LogFailedLoginAsync(user.Id, user.UserName ?? string.Empty, "MFA verrouillé");
            ModelState.AddModelError(string.Empty, "Compte temporairement verrouillé. Réessayez plus tard.");
            return View(model);
        }

        if (!result.Succeeded)
        {
            await LogFailedLoginAsync(user.Id, user.UserName ?? string.Empty, "code MFA invalide");
            ModelState.AddModelError(string.Empty, "Code invalide ou expiré.");
            return View(model);
        }

        await LogAuditAsync(user, "Login (MFA e-mail validé)");
        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)) return Redirect(model.ReturnUrl);
        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("mfa")]
    public async Task<IActionResult> ResendMfa(string? returnUrl = null)
    {
        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user is null) return RedirectToAction(nameof(Login));
        try
        {
            await SendMfaCodeAsync(user, HttpContext.RequestAborted);
            TempData["Success"] = "Un nouveau code vient d'être envoyé.";
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Impossible de renvoyer le code MFA pour {UserName}", user.UserName);
            TempData["Error"] = "Le code n'a pas pu être envoyé. Contactez l'équipe IT.";
        }
        return RedirectToAction(nameof(VerifyMfa), new { returnUrl });
    }

    private async Task SendMfaCodeAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(user.Email) || !user.EmailConfirmed)
            throw new InvalidOperationException("Le compte IT ne possède pas d'adresse e-mail confirmée.");
        var code = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
        await emailSender.SendMfaCodeAsync(user.Email, code, cancellationToken);
    }

    private async Task LogAuditAsync(ApplicationUser user, string action)
    {
        dbContext.AuditLogEntries.Add(new AuditLogEntry
        {
            Id = Guid.NewGuid(), DateUtc = DateTime.UtcNow, UserId = user.Id,
            UserName = user.UserName, Action = action
        });
        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static string MaskEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return "adresse non configurée";
        var separator = email.IndexOf('@');
        if (separator <= 1) return "***" + email[separator..];
        return email[0] + new string('*', Math.Min(separator - 1, 6)) + email[separator..];
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


