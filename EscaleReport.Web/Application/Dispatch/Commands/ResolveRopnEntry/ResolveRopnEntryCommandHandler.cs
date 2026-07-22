using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.ResolveRopnEntry;

public class ResolveRopnEntryCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ResolveRopnEntryCommand>
{
    public async Task Handle(ResolveRopnEntryCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var entry = await dbContext.RopnEntries
            .FirstOrDefaultAsync(r => r.Id == request.RopnEntryId, cancellationToken);
        if (entry is null)
        {
            return;
        }

        entry.Resoudre(request.ActionRealisee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
