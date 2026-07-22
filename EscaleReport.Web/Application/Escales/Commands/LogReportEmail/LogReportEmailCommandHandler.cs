using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Escales.Commands.LogReportEmail;

public class LogReportEmailCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<LogReportEmailCommand, string?>
{
    public async Task<string?> Handle(LogReportEmailCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.EnvoyerRapportEmail))
        {
            throw new ForbiddenAccessException(Permissions.EnvoyerRapportEmail);
        }

        var escale = await dbContext.Escales.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.EscaleId, cancellationToken);
        if (escale is null)
        {
            return null;
        }

        // Modèle de mail paramétrable "à terme" (CDC §14.5) : ici valeurs par défaut avec
        // substitution de variables {navire}/{voyage}/{ligne}, en attendant l'écran de
        // paramétrage des modèles (CDC §15.1), non encore construit.
        var subject = $"Rapport d'escale - {escale.Navire} ({escale.Voyage})";
        var body =
            $"Bonjour,\n\nVeuillez trouver ci-joint le rapport d'escale du navire {escale.Navire}, " +
            $"voyage {escale.Voyage}, ligne {escale.LigneMaritime}.\n\n" +
            "Merci de joindre le fichier PDF exporté depuis la fiche escale avant l'envoi.\n\n" +
            "Cordialement.";

        dbContext.ReportEmailLogs.Add(new ReportEmailLog
        {
            EscaleId = escale.Id,
            ReportType = "RapportEscale",
            SentBy = currentUser.UserName ?? "—",
            Recipients = request.Recipients,
            Subject = subject,
            SentAtUtc = DateTime.UtcNow
        });
        await dbContext.SaveChangesAsync(cancellationToken);

        // RFC 6068 : la liste de destinataires (avant le "?") ne s'encode pas comme les
        // paramètres de requête — un "@" encodé en %40 dérouterait certains clients mail.
        var mailto = $"mailto:{request.Recipients.Replace(" ", "")}" +
                     $"?subject={Uri.EscapeDataString(subject)}" +
                     $"&body={Uri.EscapeDataString(body)}";

        return mailto;
    }
}
