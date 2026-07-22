using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.EndStsPointeur;

public class EndStsPointeurCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<EndStsPointeurCommand>
{
    public async Task Handle(EndStsPointeurCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var pointeur = await dbContext.StsPointeurs
            .FirstOrDefaultAsync(p => p.Id == request.PointeurId, cancellationToken);
        if (pointeur is null)
        {
            return;
        }

        pointeur.TerminerService();
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
