using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.YardPlanning.Commands.EndTransfertOut;

public class EndTransfertOutCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<EndTransfertOutCommand>
{
    public async Task Handle(EndTransfertOutCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var transfert = await dbContext.TransfertsOut
            .FirstOrDefaultAsync(t => t.Id == request.TransfertId, cancellationToken);
        if (transfert is null)
        {
            return;
        }

        transfert.Terminer();
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
