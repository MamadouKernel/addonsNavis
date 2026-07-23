using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Reporting;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Reporting.Commands.ValidateShiftReport;

// CDC §2 "Shift Manager" : "Valider ou commenter le rapport, selon le workflow retenu" —
// signature de supervision, distincte de la rédaction du shift sortant (SaisirDonneesModule).
public class ValidateShiftReportCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ValidateShiftReportCommand>
{
    public async Task Handle(ValidateShiftReportCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ValiderRapport))
        {
            throw new ForbiddenAccessException(Permissions.ValiderRapport);
        }

        var note = await dbContext.ShiftHandoverNotes
            .FirstOrDefaultAsync(n => n.Date == request.Date && n.Shift == request.Shift, cancellationToken);

        if (note is null)
        {
            note = new ShiftHandoverNote { Date = request.Date, Shift = request.Shift };
            dbContext.ShiftHandoverNotes.Add(note);
        }

        note.ValidePar = currentUser.UserName;
        note.ValideLeUtc = DateTime.UtcNow;
        note.CommentaireValidation = request.CommentaireValidation;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
