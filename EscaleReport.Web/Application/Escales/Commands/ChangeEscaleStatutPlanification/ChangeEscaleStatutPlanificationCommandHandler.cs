using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Escales.Commands.ChangeEscaleStatutPlanification;

public class ChangeEscaleStatutPlanificationCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ChangeEscaleStatutPlanificationCommand>
{
    public async Task Handle(ChangeEscaleStatutPlanificationCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierEscale))
        {
            throw new ForbiddenAccessException(Permissions.ModifierEscale);
        }

        var escale = await dbContext.Escales.FirstOrDefaultAsync(e => e.Id == request.EscaleId, cancellationToken);
        if (escale is null)
        {
            return;
        }

        escale.ChangerStatutPlanification(request.NouveauStatut);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
