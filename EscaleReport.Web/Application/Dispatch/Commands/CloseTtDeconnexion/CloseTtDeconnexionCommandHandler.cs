using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseTtDeconnexion;

public class CloseTtDeconnexionCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<CloseTtDeconnexionCommand>
{
    public async Task Handle(CloseTtDeconnexionCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var deconnexion = await dbContext.TtDeconnexions
            .FirstOrDefaultAsync(d => d.Id == request.DeconnexionId, cancellationToken);
        if (deconnexion is null)
        {
            return;
        }

        deconnexion.SignalerRetour();
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
