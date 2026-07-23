using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddStsPointeur;

public class AddStsPointeurCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddStsPointeurCommand, Guid>
{
    public async Task<Guid> Handle(AddStsPointeurCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "STS"))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var pointeur = new StsPointeur
        {
            Nom = request.Nom,
            Role = request.Role,
            NavireOuZone = request.NavireOuZone,
            HeurePriseDePosteUtc = request.HeurePriseDePosteUtc
        };

        dbContext.StsPointeurs.Add(pointeur);
        await dbContext.SaveChangesAsync(cancellationToken);

        return pointeur.Id;
    }
}
