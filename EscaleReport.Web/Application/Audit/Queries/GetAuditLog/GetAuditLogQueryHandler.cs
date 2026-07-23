using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Common.Models;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Audit.Queries.GetAuditLog;

public class GetAuditLogQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetAuditLogQuery, AuditLogResultDto>
{
    public async Task<AuditLogResultDto> Handle(GetAuditLogQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterJournalAudit))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterJournalAudit);
        }

        var query = dbContext.AuditLogEntries.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.UserName))
        {
            query = query.Where(a => a.UserName == request.UserName);
        }

        if (!string.IsNullOrWhiteSpace(request.ActionType))
        {
            query = query.Where(a => a.Action == request.ActionType);
        }

        if (request.DateDebut is { } debut)
        {
            var debutUtc = debut.ToDateTime(TimeOnly.MinValue);
            query = query.Where(a => a.DateUtc >= debutUtc);
        }

        if (request.DateFin is { } fin)
        {
            var finUtc = fin.ToDateTime(TimeOnly.MaxValue);
            query = query.Where(a => a.DateUtc <= finUtc);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var (page, skip, pageSize, _) = Paging.Resolve(request.Page, request.PageSize, totalCount);

        var entries = await query
            .OrderByDescending(a => a.DateUtc)
            .Skip(skip)
            .Take(pageSize)
            .Select(a => new AuditLogEntryDto
            {
                Id = a.Id,
                DateUtc = a.DateUtc,
                UserName = a.UserName,
                Action = a.Action,
                Cible = a.Cible
            })
            .ToListAsync(cancellationToken);

        var utilisateurs = await dbContext.AuditLogEntries.AsNoTracking()
            .Where(a => a.UserName != null)
            .Select(a => a.UserName!)
            .Distinct()
            .OrderBy(u => u)
            .ToListAsync(cancellationToken);

        var actions = await dbContext.AuditLogEntries.AsNoTracking()
            .Select(a => a.Action)
            .Distinct()
            .OrderBy(a => a)
            .ToListAsync(cancellationToken);

        return new AuditLogResultDto
        {
            Entries = new PagedResult<AuditLogEntryDto>
            {
                Items = entries,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            },
            Utilisateurs = utilisateurs,
            Actions = actions
        };
    }
}
