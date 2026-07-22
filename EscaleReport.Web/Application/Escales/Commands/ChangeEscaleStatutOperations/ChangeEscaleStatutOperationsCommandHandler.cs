using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Escales.Commands.ChangeEscaleStatutOperations;

public class ChangeEscaleStatutOperationsCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ChangeEscaleStatutOperationsCommand>
{
    public async Task Handle(ChangeEscaleStatutOperationsCommand request, CancellationToken cancellationToken)
    {
        // CDC §4.3 : "Le passage au statut « Terminées » devra être réservé aux utilisateurs
        // habilités" — permission distincte et plus restrictive que la modification générale.
        var permissionRequise = request.NouveauStatut == StatutOperations.Terminees
            ? Permissions.MarquerEscaleTerminee
            : Permissions.ModifierEscale;

        if (!currentUser.HasPermission(permissionRequise))
        {
            throw new ForbiddenAccessException(permissionRequise);
        }

        var escale = await dbContext.Escales.FirstOrDefaultAsync(e => e.Id == request.EscaleId, cancellationToken);
        if (escale is null)
        {
            return;
        }

        escale.ChangerStatutOperations(request.NouveauStatut);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
