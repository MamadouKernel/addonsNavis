using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using EscaleReport.Web.Domain.Audit;
using System.Text.Json;

namespace EscaleReport.Web.Infrastructure.Persistence.Interceptors;

// Renseigne Created/UpdatedAtUtc + auteur sur toute entité auditable, sans que
// chaque Handler de l'Application n'ait à y penser (CDC §2.5 "traçabilité des actions").
public class AuditableEntitySaveChangesInterceptor(ICurrentUserService currentUser) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditFields(DbContext? context)
    {
        if (context is null) return;

        var now = DateTime.UtcNow;
        var userName = currentUser.UserName;

        var entries = context.ChangeTracker.Entries<BaseAuditableEntity>().ToList();
        foreach (EntityEntry<BaseAuditableEntity> entry in entries)
        {
            var originalState = entry.State;
            if (entry.State == EntityState.Deleted)
            {
                // Aucun Remove() métier ne doit provoquer de DELETE SQL. On conserve la ligne
                // et ses relations pour permettre une restauration complète.
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAtUtc = now;
                entry.Entity.DeletedBy = userName;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAtUtc = now;
                entry.Entity.CreatedBy = userName;
            }

            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.UpdatedAtUtc = now;
                entry.Entity.UpdatedBy = userName;
                entry.Entity.Version = entry.State == EntityState.Added ? 1 : entry.Entity.Version + 1;
            }

            var changes = BuildChanges(entry, originalState);
            if (changes.Count > 0)
            {
                context.Set<AuditLogEntry>().Add(new AuditLogEntry
                {
                    Id = Guid.NewGuid(),
                    DateUtc = now,
                    UserId = currentUser.UserId,
                    UserName = userName,
                    Action = originalState switch
                    {
                        EntityState.Added => "EntityCreated",
                        EntityState.Deleted => "EntitySoftDeleted",
                        _ => "EntityUpdated"
                    },
                    Cible = entry.Entity.Id.ToString(),
                    EntityType = entry.Metadata.ClrType.Name,
                    EntityId = entry.Entity.Id.ToString(),
                    ChangesJson = JsonSerializer.Serialize(changes)
                });
            }
        }
    }

    private static Dictionary<string, object?> BuildChanges(EntityEntry<BaseAuditableEntity> entry, EntityState originalState)
    {
        var ignored = new HashSet<string>(StringComparer.Ordinal)
        {
            nameof(BaseAuditableEntity.CreatedAtUtc), nameof(BaseAuditableEntity.CreatedBy),
            nameof(BaseAuditableEntity.UpdatedAtUtc), nameof(BaseAuditableEntity.UpdatedBy),
            nameof(BaseAuditableEntity.DeletedAtUtc), nameof(BaseAuditableEntity.DeletedBy)
        };
        var result = new Dictionary<string, object?>();
        foreach (var property in entry.Properties.Where(p => !ignored.Contains(p.Metadata.Name)))
        {
            if (originalState == EntityState.Modified && !property.IsModified) continue;
            if (originalState == EntityState.Added)
                result[property.Metadata.Name] = new { Before = (object?)null, After = property.CurrentValue };
            else if (originalState == EntityState.Deleted && property.Metadata.Name == nameof(BaseAuditableEntity.IsDeleted))
                result[property.Metadata.Name] = new { Before = (object?)false, After = (object?)true };
            else if (originalState == EntityState.Modified)
                result[property.Metadata.Name] = new { Before = property.OriginalValue, After = property.CurrentValue };
        }
        return result;
    }
}
