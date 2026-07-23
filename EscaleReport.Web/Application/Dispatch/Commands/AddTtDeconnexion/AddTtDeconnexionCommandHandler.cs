using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddTtDeconnexion;

public class AddTtDeconnexionCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddTtDeconnexionCommand, Guid>
{
    public async Task<Guid> Handle(AddTtDeconnexionCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "TT"))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var deconnexion = new TtDeconnexion
        {
            NumeroTt = request.NumeroTt,
            DateDebutUtc = request.DateDebutUtc,
            Raison = request.Raison,
            RetireEffectif = request.RetireEffectif
        };

        dbContext.TtDeconnexions.Add(deconnexion);
        await dbContext.SaveChangesAsync(cancellationToken);

        return deconnexion.Id;
    }
}
