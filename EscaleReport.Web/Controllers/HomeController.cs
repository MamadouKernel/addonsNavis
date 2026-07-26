using System.Diagnostics;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using EscaleReport.Web.Models;
using EscaleReport.Web.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Controllers;

public class HomeController : Controller
{
    // Reste accessible anonymement : référencé par app.UseExceptionHandler("/Home/Error")
    // dans Program.cs, y compris pour une exception levée avant authentification.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? statusCode = null)
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        var code = statusCode ?? exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            ForbiddenAccessException or UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
            DbUpdateException or DbException => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status500InternalServerError
        };

        var presentation = Present(code);
        Response.StatusCode = code;
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            StatusCode = code,
            Title = presentation.Title,
            Message = presentation.Message,
            ActionLabel = presentation.ActionLabel,
            ActionUrl = presentation.ActionUrl,
            CanRetry = presentation.CanRetry
        });
    }

    private static (string Title, string Message, string ActionLabel, string ActionUrl, bool CanRetry) Present(int code) => code switch
    {
        StatusCodes.Status400BadRequest => ("Demande à corriger", "Certaines informations sont invalides ou incomplètes. Vérifiez votre saisie puis réessayez.", "Retour aux escales", "/Escales", false),
        StatusCodes.Status401Unauthorized => ("Connexion nécessaire", "Votre session est absente ou a expiré. Reconnectez-vous pour continuer.", "Se connecter", "/Account/Login", false),
        StatusCodes.Status403Forbidden => ("Accès refusé", "Votre rôle ou vos permissions ne permettent pas d'accéder à cette ressource.", "Retour aux escales", "/Escales", false),
        StatusCodes.Status404NotFound => ("Page introuvable", "La page demandée n'existe pas, a été déplacée ou la ressource n'est plus disponible.", "Retour aux escales", "/Escales", false),
        StatusCodes.Status409Conflict => ("Modification concurrente", "Les données ont changé depuis l'ouverture de la page. Rechargez-les avant de recommencer.", "Retour aux escales", "/Escales", true),
        StatusCodes.Status429TooManyRequests => ("Trop de demandes", "Le nombre de tentatives autorisées a été dépassé. Patientez une minute avant de réessayer.", "Retour à la connexion", "/Account/Login", true),
        StatusCodes.Status503ServiceUnavailable => ("Service temporairement indisponible", "La base de données ou un service nécessaire ne répond pas. Réessayez dans quelques instants.", "Retour aux escales", "/Escales", true),
        _ => ("Incident inattendu", "Nous n'avons pas pu terminer cette opération. Aucune information technique sensible n'a été affichée.", "Retour aux escales", "/Escales", true)
    };
}
