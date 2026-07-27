using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddRopnEntry;

public class AddRopnEntryCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddRopnEntryCommand, Guid>
{
    public async Task<Guid> Handle(AddRopnEntryCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "STS"))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var entry = new RopnEntry
        {
            Nom = request.Nom,
            Role = request.Role,
            DateDebutUtc = request.DateDebutUtc == default ? DateTime.UtcNow : request.DateDebutUtc,
            DateFinUtc = request.DateFinUtc,
            DifficulteRencontree = request.DifficulteRencontree
        };

        dbContext.RopnEntries.Add(entry);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entry.Id;
    }
}
