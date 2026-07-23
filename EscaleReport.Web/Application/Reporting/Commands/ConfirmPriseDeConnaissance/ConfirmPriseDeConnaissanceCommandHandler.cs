using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Reporting;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Reporting.Commands.ConfirmPriseDeConnaissance;

// CDC §18 "Transmission entre shifts" : "Le shift entrant devra pouvoir confirmer la prise
// de connaissance."
public class ConfirmPriseDeConnaissanceCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ConfirmPriseDeConnaissanceCommand>
{
    public async Task Handle(ConfirmPriseDeConnaissanceCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var note = await dbContext.ShiftHandoverNotes
            .FirstOrDefaultAsync(n => n.Date == request.Date && n.Shift == request.Shift, cancellationToken);

        if (note is null)
        {
            note = new ShiftHandoverNote { Date = request.Date, Shift = request.Shift };
            dbContext.ShiftHandoverNotes.Add(note);
        }

        note.PriseDeConnaissanceParUtilisateur = currentUser.UserName;
        note.PriseDeConnaissanceLeUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
