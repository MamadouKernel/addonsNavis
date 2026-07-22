using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddDangerousContainer;

public class AddDangerousContainerCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddDangerousContainerCommand, Guid>
{
    public async Task<Guid> Handle(AddDangerousContainerCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var container = new Domain.VesselPlanning.DangerousContainer
        {
            EscaleId = request.EscaleId,
            NumeroConteneur = request.NumeroConteneur,
            LigneMaritime = request.LigneMaritime,
            ClasseImo = request.ClasseImo,
            Position = request.Position,
            DateValiditeBadt = request.DateValiditeBadt
        };

        dbContext.DangerousContainers.Add(container);
        await dbContext.SaveChangesAsync(cancellationToken);

        return container.Id;
    }
}
