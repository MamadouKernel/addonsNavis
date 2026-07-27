using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Escales.Commands.UpdateEscale;

public class UpdateEscaleCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateEscaleCommand>
{
    public async Task Handle(UpdateEscaleCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierEscale))
        {
            throw new ForbiddenAccessException(Permissions.ModifierEscale);
        }

        var escale = await dbContext.Escales
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Escale introuvable.");

        escale.Navire = request.Navire.Trim();
        escale.Voyage = request.Voyage?.Trim() ?? string.Empty;
        escale.LigneMaritime = request.LigneMaritime?.Trim() ?? string.Empty;
        escale.Eta = request.Eta ?? default;
        escale.Ata = request.Ata;
        escale.Etc = request.Etc;
        escale.VesselVisit = request.VesselVisit?.Trim();
        escale.Quai = request.Quai?.Trim();
        escale.Shift = request.Shift?.Trim();
        escale.Planificateur = request.Planificateur?.Trim();
        escale.RefreshDraftState();

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
