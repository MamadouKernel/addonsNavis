using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Itt;
using MediatR;

namespace EscaleReport.Web.Application.Itt.Commands.AddIttTransfer;

public class AddIttTransferCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddIttTransferCommand, Guid>
{
    public async Task<Guid> Handle(AddIttTransferCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var transfer = new IttTransfer
        {
            NavireConnexion = request.NavireConnexion,
            NombreATransferer = request.NombreATransferer,
            NombreTransfere = request.NombreTransfere,
            NombreRecu = request.NombreRecu,
            Observations = request.Observations
        };

        dbContext.IttTransfers.Add(transfer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return transfer.Id;
    }
}
