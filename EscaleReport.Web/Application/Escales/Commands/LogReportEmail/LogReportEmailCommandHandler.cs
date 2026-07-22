using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Settings;
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

        // CDC §14.5/§15.1 : modèle paramétré (Sujet/Corps avec {navire}/{voyage}/{ligne}/
        // {visite}) s'il existe, sinon valeurs par défaut codées en dur.
        var template = await dbContext.EmailTemplates.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Cle == EmailTemplateKeys.RapportEscale, cancellationToken);

        string subject, body;
        if (template is not null && !string.IsNullOrWhiteSpace(template.Sujet))
        {
            subject = SubstituteVariables(template.Sujet, escale);
            body = SubstituteVariables(template.Corps, escale);
        }
        else
        {
            subject = $"Rapport d'escale - {escale.Navire} ({escale.Voyage})";
            body =
                $"Bonjour,\n\nVeuillez trouver ci-joint le rapport d'escale du navire {escale.Navire}, " +
                $"voyage {escale.Voyage}, ligne {escale.LigneMaritime}.\n\n" +
                "Merci de joindre le fichier PDF exporté depuis la fiche escale avant l'envoi.\n\n" +
                "Cordialement.";
        }

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

    private static string SubstituteVariables(string text, Escale escale) => text
        .Replace("{navire}", escale.Navire)
        .Replace("{voyage}", escale.Voyage)
        .Replace("{ligne}", escale.LigneMaritime)
        .Replace("{visite}", escale.VesselVisit ?? "");
}
