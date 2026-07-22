using EscaleReport.Web.Application.Coordination.Commands.AddCoordinatorIncident;
using EscaleReport.Web.Application.Coordination.Commands.CloseCoordinatorIncident;
using EscaleReport.Web.Application.Coordination.Queries.GetCoordinatorDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Module Coordinateur Control Room (CDC §12).
[Authorize]
public class CoordinationController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(new GetCoordinatorDashboardQuery(), cancellationToken);
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddIncident(AddCoordinatorIncidentCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseIncident(Guid incidentId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseCoordinatorIncidentCommand(incidentId, actionRealisee), cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
