using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Itt.Commands.UpdateIttTransfer;

public class UpdateIttTransferCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateIttTransferCommand>
{
    public async Task Handle(UpdateIttTransferCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.ModifierDonneesModule);
        }

        var transfer = await dbContext.IttTransfers
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (transfer is null)
        {
            return;
        }

        transfer.NombreATransferer = request.NombreATransferer;
        transfer.NombreTransfere = request.NombreTransfere;
        transfer.NombreRecu = request.NombreRecu;
        transfer.Observations = request.Observations;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
