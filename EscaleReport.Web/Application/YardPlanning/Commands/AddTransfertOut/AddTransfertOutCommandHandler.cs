using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.YardPlanning;
using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Commands.AddTransfertOut;

public class AddTransfertOutCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddTransfertOutCommand, Guid>
{
    public async Task<Guid> Handle(AddTransfertOutCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var transfert = new TransfertOut
        {
            Bay = request.Bay,
            NombreConteneurs = request.NombreConteneurs,
            DetailOuDestination = request.DetailOuDestination,
            HeureDebut = request.HeureDebut,
            Commentaire = request.Commentaire
        };

        dbContext.TransfertsOut.Add(transfert);
        await dbContext.SaveChangesAsync(cancellationToken);

        return transfert.Id;
    }
}
