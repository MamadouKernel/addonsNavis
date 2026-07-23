using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Audit;
using MediatR;

namespace EscaleReport.Web.Application.Common.Behaviours;

// Alimente automatiquement le journal d'audit (CDC §2 "historique de ses actions", §18
// traçabilité) pour toute commande qui s'exécute avec succès — aucune commande n'a besoin
// d'opter explicitement, ce qui évite d'oublier la traçabilité sur un futur handler. Les
// requêtes de lecture (Query) ne sont volontairement pas journalisées en succès : seules les
// actions qui modifient l'état du système constituent une "action" au sens du CDC. Un refus
// d'autorisation (Command ou Query) est en revanche toujours journalisé : c'est un événement
// de sécurité, pas une simple lecture.
public class AuditLoggingBehaviour<TRequest, TResponse>(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        TResponse response;

        try
        {
            response = await next(cancellationToken);
        }
        catch (ForbiddenAccessException)
        {
            dbContext.AuditLogEntries.Add(new AuditLogEntry
            {
                Id = Guid.NewGuid(),
                DateUtc = DateTime.UtcNow,
                UserId = currentUser.UserId,
                UserName = currentUser.UserName,
                Action = $"{requestName} (refusé)",
                Cible = ExtractCible(request)
            });
            await dbContext.SaveChangesAsync(cancellationToken);
            throw;
        }

        if (requestName.EndsWith("Command", StringComparison.Ordinal))
        {
            dbContext.AuditLogEntries.Add(new AuditLogEntry
            {
                Id = Guid.NewGuid(),
                DateUtc = DateTime.UtcNow,
                UserId = currentUser.UserId,
                UserName = currentUser.UserName,
                Action = requestName,
                Cible = ExtractCible(request)
            });
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return response;
    }

    private static string? ExtractCible(TRequest request)
    {
        var props = typeof(TRequest).GetProperties();
        foreach (var name in new[] { "EscaleId", "Id", "UserId" })
        {
            var value = props.FirstOrDefault(p => p.Name == name)?.GetValue(request);
            if (value is not null)
            {
                return value.ToString();
            }
        }

        return null;
    }
}
