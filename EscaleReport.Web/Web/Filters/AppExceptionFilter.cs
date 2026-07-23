using EscaleReport.Web.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EscaleReport.Web.Web.Filters;

// Convertit en message convivial (même mécanisme que ConcurrencyExceptionFilter) les deux
// exceptions métier qui remontaient jusqu'ici en page d'erreur brute : un refus d'autorisation
// (permission ou cloisonnement par poste, CDC §2/§27) et un échec de validation FluentValidation
// qui n'a pas été intercepté plus tôt (ex. saisie via un appel direct au endpoint POST).
public class AppExceptionFilter(ITempDataDictionaryFactory tempDataFactory) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var message = context.Exception switch
        {
            ForbiddenAccessException => "Vous n'avez pas la permission nécessaire pour effectuer cette action.",
            ValidationException validationException => string.Join(
                " ", validationException.Errors.Select(e => e.ErrorMessage)),
            _ => null
        };

        if (message is null)
        {
            return;
        }

        var tempData = tempDataFactory.GetTempData(context.HttpContext);
        tempData["Error"] = message;

        context.Result = new RedirectResult(SafeRedirect.ToLocalReferer(context.HttpContext));
        context.ExceptionHandled = true;
    }
}
