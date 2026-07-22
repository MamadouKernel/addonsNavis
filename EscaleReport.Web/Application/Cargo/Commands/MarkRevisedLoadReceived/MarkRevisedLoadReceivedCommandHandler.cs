using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Cargo;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Cargo.Commands.MarkRevisedLoadReceived;

public class MarkRevisedLoadReceivedCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<MarkRevisedLoadReceivedCommand>
{
    public async Task Handle(MarkRevisedLoadReceivedCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var consommation = await dbContext.CargoConsommations
            .FirstOrDefaultAsync(c => c.EscaleId == request.EscaleId, cancellationToken);

        if (consommation is null)
        {
            consommation = new CargoConsommation { EscaleId = request.EscaleId };
            dbContext.CargoConsommations.Add(consommation);
        }

        // CDC §10.6 : date/heure et confirmateur enregistrés automatiquement.
        consommation.MarquerRevisedLoadRecu(currentUser.UserName, request.Observations);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
