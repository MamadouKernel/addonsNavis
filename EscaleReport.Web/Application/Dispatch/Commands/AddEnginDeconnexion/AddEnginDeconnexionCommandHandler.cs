using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddEnginDeconnexion;

public class AddEnginDeconnexionCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddEnginDeconnexionCommand, Guid>
{
    public async Task<Guid> Handle(AddEnginDeconnexionCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var deconnexion = new EnginDeconnexion
        {
            Engin = request.Engin,
            DateDebutUtc = request.DateDebutUtc,
            Motif = request.Motif
        };

        dbContext.EnginDeconnexions.Add(deconnexion);
        await dbContext.SaveChangesAsync(cancellationToken);

        return deconnexion.Id;
    }
}
