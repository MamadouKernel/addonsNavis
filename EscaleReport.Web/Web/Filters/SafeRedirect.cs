namespace EscaleReport.Web.Web.Filters;

// Le Referer HTTP est une donnée envoyée par le client, donc non fiable : rediriger dessus sans
// vérification permet à un lien externe de renvoyer un utilisateur vers un site tiers juste
// après confirmation qu'il est connecté (open redirect / hameçonnage). On ne l'utilise que s'il
// pointe vers le même hôte que la requête en cours ; sinon on retombe sur "/".
public static class SafeRedirect
{
    public static string ToLocalReferer(HttpContext context)
    {
        var referer = context.Request.Headers.Referer.ToString();
        if (!string.IsNullOrEmpty(referer) &&
            Uri.TryCreate(referer, UriKind.Absolute, out var refererUri) &&
            string.Equals(refererUri.Host, context.Request.Host.Host, StringComparison.OrdinalIgnoreCase))
        {
            return refererUri.PathAndQuery;
        }

        return "/";
    }
}
