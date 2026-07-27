using EscaleReport.Web.Domain.Identity;
using System.Text;
using EscaleReport.Web.Application.Notifications.Queries.GetActiveAlerts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Notifications et alertes (CDC §16) — calculées en direct, affichées dans l'application et
// sur le tableau de bord ; envoi par e-mail via mailto: (même principe que le rapport
// d'escale, §14.5). Microsoft Teams explicitement hors périmètre ("sous réserve de faisabilité").
[Authorize(Roles = RoleAccessGroups.Notifications)]
public class NotificationsController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(new GetActiveAlertsQuery(), cancellationToken);
        return View(dto);
    }

    public async Task<IActionResult> SendByEmail(CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(new GetActiveAlertsQuery(), cancellationToken);

        var subject = $"EscaleReport — {dto.Alertes.Count} alerte(s) active(s)";
        var body = new StringBuilder();
        foreach (var a in dto.Alertes)
        {
            body.AppendLine($"[{a.Type}] {a.Message}");
        }

        var mailto = $"mailto:?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body.ToString())}";
        return Redirect(mailto);
    }
}
