namespace EscaleReport.Web.Infrastructure.Identity;

public sealed class AuthenticationEmailOptions
{
    public const string SectionName = "AuthenticationEmail";
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; } = 587;
    public bool EnableSsl { get; init; } = true;
    public string FromAddress { get; init; } = string.Empty;
    public string FromName { get; init; } = "EscaleReport";
    public string? UserName { get; init; }
    public string? Password { get; init; }
}
