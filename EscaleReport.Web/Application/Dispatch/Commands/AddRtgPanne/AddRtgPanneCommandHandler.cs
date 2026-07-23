using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddRtgPanne;

public class AddRtgPanneCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddRtgPanneCommand, Guid>
{
    public async Task<Guid> Handle(AddRtgPanneCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "RTG"))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var panne = new RtgPanne
        {
            Engin = request.Engin,
            DateDebutUtc = request.DateDebutUtc,
            Raison = request.Raison,
            RetireEffectif = request.RetireEffectif
        };

        dbContext.RtgPannes.Add(panne);
        await dbContext.SaveChangesAsync(cancellationToken);

        return panne.Id;
    }
}
