using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Itt.Commands.CloseIttEnginPanne;

public class CloseIttEnginPanneCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<CloseIttEnginPanneCommand>
{
    public async Task Handle(CloseIttEnginPanneCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var panne = await dbContext.IttEnginPannes
            .FirstOrDefaultAsync(p => p.Id == request.PanneId, cancellationToken);
        if (panne is null)
        {
            return;
        }

        panne.Cloturer(request.ActionRealisee);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
