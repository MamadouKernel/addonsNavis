using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseRtgClash;

public class CloseRtgClashCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<CloseRtgClashCommand>
{
    public async Task Handle(CloseRtgClashCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "RTG"))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var clash = await dbContext.RtgClashes
            .FirstOrDefaultAsync(c => c.Id == request.ClashId, cancellationToken);
        if (clash is null)
        {
            return;
        }

        clash.Cloturer(request.ActionRealisee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
