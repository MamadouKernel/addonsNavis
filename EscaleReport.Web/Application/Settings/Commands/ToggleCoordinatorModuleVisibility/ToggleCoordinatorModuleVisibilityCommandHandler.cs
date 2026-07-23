using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Settings.Commands.ToggleCoordinatorModuleVisibility;

public class ToggleCoordinatorModuleVisibilityCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ToggleCoordinatorModuleVisibilityCommand>
{
    public async Task Handle(ToggleCoordinatorModuleVisibilityCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierParametres))
        {
            throw new ForbiddenAccessException(Permissions.ModifierParametres);
        }

        var visibility = await dbContext.CoordinatorModuleVisibilities
            .FirstOrDefaultAsync(v => v.Cle == request.Cle, cancellationToken);

        if (visibility is null)
        {
            // Absent = visible par défaut (voir CoordinatorModuleVisibility) : la première
            // bascule d'un module encore jamais touché doit donc le masquer, pas le "réafficher".
            visibility = new CoordinatorModuleVisibility { Cle = request.Cle, EstVisible = false };
            dbContext.CoordinatorModuleVisibilities.Add(visibility);
        }
        else
        {
            visibility.EstVisible = !visibility.EstVisible;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
