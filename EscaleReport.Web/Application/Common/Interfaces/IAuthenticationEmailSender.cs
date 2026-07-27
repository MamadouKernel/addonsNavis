namespace EscaleReport.Web.Application.Common.Interfaces;

public interface IAuthenticationEmailSender
{
    Task SendMfaCodeAsync(string recipient, string code, CancellationToken cancellationToken = default);
}
