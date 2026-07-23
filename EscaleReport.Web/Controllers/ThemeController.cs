using EscaleReport.Web.Web.Theming;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// CDC §15.2 "Personnalisation de l'interface" : mode clair / mode sombre, choix persistant
// de l'utilisateur (cookie), indépendant des préférences système du poste — pertinent en
// Control Room où plusieurs utilisateurs se succèdent sur un même poste partagé.
[Authorize]
public class ThemeController : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Toggle(string? returnUrl)
    {
        var current = Request.Cookies["theme"];
        var next = current == "dark" ? "light" : "dark";

        Response.Cookies.Append("theme", next, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            HttpOnly = false,
            SameSite = SameSiteMode.Lax
        });

        return LocalRedirect(string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl) ? "/" : returnUrl);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetPalette(string palette, string? returnUrl)
    {
        Response.Cookies.Append("palette", PaletteCatalog.FromCookie(palette), new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            HttpOnly = false,
            SameSite = SameSiteMode.Lax
        });

        return LocalRedirect(string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl) ? "/" : returnUrl);
    }
}
