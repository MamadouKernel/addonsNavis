using System.Security.Cryptography;

namespace EscaleReport.Web.Web.Security;

// Un nonce CSP par requête permet de retirer 'unsafe-inline' du script-src (voir Program.cs) :
// seuls les <script> portant ce nonce s'exécutent, ce qui bloque un <script> injecté par un
// utilisateur malveillant. Stocké dans HttpContext.Items pour rester identique entre le
// middleware qui pose l'en-tête CSP et les vues qui l'attachent aux balises <script>.
public static class CspNonceExtensions
{
    private const string ItemsKey = "csp-nonce";

    public static string GetOrCreateCspNonce(this HttpContext context)
    {
        if (context.Items.TryGetValue(ItemsKey, out var existing) && existing is string nonce)
        {
            return nonce;
        }

        var value = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        context.Items[ItemsKey] = value;
        return value;
    }
}
