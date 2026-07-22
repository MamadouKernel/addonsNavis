using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.CloseRtgPanne;

public class CloseRtgPanneCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<CloseRtgPanneCommand>
{
    public async Task Handle(CloseRtgPanneCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var panne = await dbContext.RtgPannes
            .FirstOrDefaultAsync(p => p.Id == request.PanneId, cancellationToken);
        if (panne is null)
        {
            return;
        }

        panne.Cloturer(request.CommentaireReprise);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
