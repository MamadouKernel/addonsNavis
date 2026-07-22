using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddRemplacementOperateur;

public class AddRemplacementOperateurCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddRemplacementOperateurCommand, Guid>
{
    public async Task<Guid> Handle(AddRemplacementOperateurCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var remplacement = new RemplacementOperateur
        {
            Operateur = request.Operateur,
            EnginQuitte = request.EnginQuitte,
            NouvelEngin = request.NouvelEngin,
            DateHeureUtc = request.DateHeureUtc,
            Raison = request.Raison,
            Commentaire = request.Commentaire
        };

        dbContext.RemplacementsOperateur.Add(remplacement);
        await dbContext.SaveChangesAsync(cancellationToken);

        return remplacement.Id;
    }
}
