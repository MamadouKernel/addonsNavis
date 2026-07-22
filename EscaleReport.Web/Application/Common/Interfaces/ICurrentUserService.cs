namespace EscaleReport.Web.Application.Common.Interfaces;

// Le contrôle des permissions doit être rejoué côté serveur (CDC §27) : l'Application
// interroge ce service, jamais l'état affiché côté client.
public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? UserName { get; }

    bool HasPermission(string permissionKey);
    bool IsInRole(string role);
}
