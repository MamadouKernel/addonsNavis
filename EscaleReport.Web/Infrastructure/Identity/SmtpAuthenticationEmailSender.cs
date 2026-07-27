using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.Encodings.Web;
using EscaleReport.Web.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace EscaleReport.Web.Infrastructure.Identity;

public sealed class SmtpAuthenticationEmailSender(
    IOptions<AuthenticationEmailOptions> options,
    ILogger<SmtpAuthenticationEmailSender> logger) : IAuthenticationEmailSender
{
    public async Task SendMfaCodeAsync(string recipient, string code, CancellationToken cancellationToken = default)
    {
        var settings = options.Value;
        if (string.IsNullOrWhiteSpace(settings.Host) || string.IsNullOrWhiteSpace(settings.FromAddress))
        {
            throw new InvalidOperationException("Le serveur d'envoi des codes MFA n'est pas configuré.");
        }

        var encodedCode = HtmlEncoder.Default.Encode(code);
        var html = BuildHtmlTemplate(encodedCode);
        var plainText = $"Code de sécurité EscaleReport : {code}\n\nCe code est personnel et à usage unique. Si vous n'êtes pas à l'origine de cette connexion, contactez immédiatement l'équipe IT.";

        using var message = new MailMessage
        {
            From = new MailAddress(settings.FromAddress, settings.FromName),
            Subject = $"{code} — votre code de sécurité EscaleReport",
            Body = html,
            IsBodyHtml = true
        };
        message.To.Add(recipient);
        message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plainText, null, MediaTypeNames.Text.Plain));
        message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

        using var client = new SmtpClient(settings.Host, settings.Port)
        {
            EnableSsl = settings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Timeout = 15000
        };
        if (!string.IsNullOrWhiteSpace(settings.UserName))
        {
            client.Credentials = new NetworkCredential(settings.UserName, settings.Password);
        }

        await client.SendMailAsync(message, cancellationToken);
        logger.LogInformation("Code MFA envoyé à l'adresse associée au compte.");
    }

    private static string BuildHtmlTemplate(string code) => $$"""
        <!doctype html>
        <html lang="fr">
        <head><meta charset="utf-8"><meta name="viewport" content="width=device-width"></head>
        <body style="margin:0;padding:0;background:#eef7f7;font-family:Segoe UI,Arial,sans-serif;color:#092f35">
          <table role="presentation" width="100%" cellspacing="0" cellpadding="0" border="0" style="background:#eef7f7">
            <tr><td align="center" style="padding:32px 12px">
              <table role="presentation" width="600" cellspacing="0" cellpadding="0" border="0" style="width:100%;max-width:600px;background:#ffffff;border-radius:20px;overflow:hidden;box-shadow:0 12px 32px rgba(9,47,53,.12)">
                <tr><td style="padding:28px 32px;background:#063e46;color:#ffffff">
                  <div style="font-size:24px;font-weight:700">EscaleReport</div>
                  <div style="margin-top:5px;font-size:12px;letter-spacing:1.4px;color:#a9dfe0;text-transform:uppercase">Côte d’Ivoire Terminal</div>
                </td></tr>
                <tr><td style="padding:36px 32px">
                  <div style="display:inline-block;padding:7px 12px;border-radius:999px;background:#d9f5f1;color:#0c756f;font-size:12px;font-weight:700;text-transform:uppercase;letter-spacing:.7px">Vérification de sécurité</div>
                  <h1 style="margin:22px 0 10px;font-size:26px;line-height:1.25;color:#092f35">Confirmez votre connexion</h1>
                  <p style="margin:0 0 24px;font-size:16px;line-height:1.6;color:#4d6f74">Utilisez le code ci-dessous pour terminer votre connexion Administrateur IT.</p>
                  <div style="padding:22px;text-align:center;border:1px solid #b9e3e1;border-radius:16px;background:#f3fbfa;font-size:34px;font-weight:800;letter-spacing:10px;color:#063e46">{{code}}</div>
                  <p style="margin:24px 0 0;font-size:14px;line-height:1.6;color:#66858a">Ce code est personnel et à usage unique. Ne le communiquez à personne.</p>
                  <div style="margin-top:24px;padding:16px;border-left:4px solid #e6a23c;background:#fff8eb;color:#6b4a13;font-size:13px;line-height:1.5">Vous n’êtes pas à l’origine de cette connexion ? Contactez immédiatement l’équipe IT.</div>
                </td></tr>
                <tr><td style="padding:20px 32px;background:#f5f8f8;color:#718b8f;font-size:12px;text-align:center">Message automatique de sécurité — merci de ne pas répondre.</td></tr>
              </table>
            </td></tr>
          </table>
        </body>
        </html>
        """;
}
