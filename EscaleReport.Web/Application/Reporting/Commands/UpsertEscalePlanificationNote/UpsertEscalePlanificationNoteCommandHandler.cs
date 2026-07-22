using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Reporting;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Reporting.Commands.UpsertEscalePlanificationNote;

public class UpsertEscalePlanificationNoteCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpsertEscalePlanificationNoteCommand>
{
    public async Task Handle(UpsertEscalePlanificationNoteCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var note = await dbContext.EscalePlanificationNotes
            .FirstOrDefaultAsync(n => n.EscaleId == request.EscaleId, cancellationToken);

        if (note is null)
        {
            note = new EscalePlanificationNote { EscaleId = request.EscaleId };
            dbContext.EscalePlanificationNotes.Add(note);
        }

        note.Commentaire = request.Commentaire;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
