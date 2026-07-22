using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Reporting;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Reporting.Commands.UpsertShiftHandoverNote;

public class UpsertShiftHandoverNoteCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpsertShiftHandoverNoteCommand>
{
    public async Task Handle(UpsertShiftHandoverNoteCommand request, CancellationToken cancellationToken)
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

        note.ActionsEnCours = request.ActionsEnCours;
        note.PointsATransmettre = request.PointsATransmettre;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
