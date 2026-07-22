using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddAdditionalContainer;

public class AddAdditionalContainerCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddAdditionalContainerCommand, Guid>
{
    public async Task<Guid> Handle(AddAdditionalContainerCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var container = new Domain.VesselPlanning.AdditionalContainer
        {
            EscaleId = request.EscaleId,
            NumeroConteneur = request.NumeroConteneur,
            LigneMaritime = request.LigneMaritime,
            Position = request.Position,
            Sens = request.Sens,
            Commentaire = request.Commentaire,
            ReferenceEmail = request.ReferenceEmail
        };

        dbContext.AdditionalContainers.Add(container);
        await dbContext.SaveChangesAsync(cancellationToken);

        return container.Id;
    }
}
