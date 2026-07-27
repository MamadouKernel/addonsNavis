namespace EscaleReport.Web.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
    public string Title { get; set; } = "Une erreur est survenue";
    public string Message { get; set; } = "Le service n'a pas pu traiter votre demande.";
    public string ActionLabel { get; set; } = "Retour aux escales";
    public string ActionUrl { get; set; } = "/Escales";
    public bool CanRetry { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
