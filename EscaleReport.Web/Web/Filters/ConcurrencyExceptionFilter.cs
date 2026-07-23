using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Web.Filters;

// CDC §18 "Gestion des modifications simultanées" : "Éviter l'écrasement silencieux des
// données" (déjà garanti structurellement par UpdatedAtUtc en jeton de concurrence sur les
// entités) + "Indiquer qu'une donnée a été modifiée par un autre utilisateur" — ce filtre
// global convertit l'exception EF Core brute en message convivial, sans devoir instrumenter
// chaque handler individuellement.
public class ConcurrencyExceptionFilter(ITempDataDictionaryFactory tempDataFactory) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not DbUpdateConcurrencyException)
        {
            return;
        }

        var tempData = tempDataFactory.GetTempData(context.HttpContext);
        tempData["Error"] = "Cette fiche a été modifiée entre-temps par un autre utilisateur. " +
            "Veuillez recharger la page et réappliquer votre modification.";

        context.Result = new RedirectResult(SafeRedirect.ToLocalReferer(context.HttpContext));
        context.ExceptionHandled = true;
    }
}
