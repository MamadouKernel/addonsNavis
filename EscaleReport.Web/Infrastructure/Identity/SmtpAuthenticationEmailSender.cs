using System.Net;
using System.Net.Mail;
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

        using var message = new MailMessage
        {
            From = new MailAddress(settings.FromAddress, settings.FromName),
            Subject = "Votre code de sécurité EscaleReport",
            Body = $"Votre code de sécurité est : {code}\n\nIl est personnel et à usage unique. Si vous n'êtes pas à l'origine de cette connexion, contactez immédiatement l'équipe IT.",
            IsBodyHtml = false
        };
        message.To.Add(recipient);

        using var client = new SmtpClient(settings.Host, settings.Port)
        {
            EnableSsl = settings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = string.IsNullOrWhiteSpace(settings.UserName),
            Credentials = string.IsNullOrWhiteSpace(settings.UserName)
                ? CredentialCache.DefaultNetworkCredentials
                : new NetworkCredential(settings.UserName, settings.Password),
            Timeout = 15000
        };

        await client.SendMailAsync(message, cancellationToken);
        logger.LogInformation("Code MFA envoyé à l'adresse associée au compte.");
    }
}
