using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseEnginProbleme;

public class CloseEnginProblemeCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<CloseEnginProblemeCommand>
{
    public async Task Handle(CloseEnginProblemeCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "Autres engins"))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var probleme = await dbContext.EnginProblemes
            .FirstOrDefaultAsync(p => p.Id == request.ProblemeId, cancellationToken);
        if (probleme is null)
        {
            return;
        }

        probleme.Cloturer(request.ActionRealisee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
