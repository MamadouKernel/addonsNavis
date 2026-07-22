using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Escales.Commands.CreateEscale;

public class CreateEscaleCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<CreateEscaleCommand, Guid>
{
    public async Task<Guid> Handle(CreateEscaleCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.CreerEscale))
        {
            throw new ForbiddenAccessException(Permissions.CreerEscale);
        }

        var escale = new Escale
        {
            Navire = request.Navire,
            Voyage = request.Voyage ?? string.Empty,
            LigneMaritime = request.LigneMaritime ?? string.Empty,
            Eta = request.Eta ?? default,
            VesselVisit = request.VesselVisit,
            Quai = request.Quai,
            Shift = request.Shift,
            Planificateur = request.Planificateur
        };
        escale.RefreshDraftState();

        dbContext.Escales.Add(escale);
        await dbContext.SaveChangesAsync(cancellationToken);

        return escale.Id;
    }
}
