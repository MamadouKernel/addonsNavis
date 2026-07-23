using EscaleReport.Web.Application.Audit.Queries.GetAuditLog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Journal d'audit (CDC §2 / §18) — accès réservé à la permission ConsulterJournalAudit.
[Authorize]
public class AuditController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(
        string? userName, string? actionType, DateOnly? dateDebut, DateOnly? dateFin, int page = 1,
        CancellationToken cancellationToken = default)
    {
        var dto = await mediator.Send(
            new GetAuditLogQuery(userName, actionType, dateDebut, dateFin, page), cancellationToken);
        return View(dto);
    }
}
