using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Application.Coordination.Commands.AddCoordinatorIncident;
using EscaleReport.Web.Application.Coordination.Commands.CloseCoordinatorIncident;
using EscaleReport.Web.Application.Coordination.Queries.GetCoordinatorDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Module Coordinateur Control Room (CDC §12).
[Authorize(Roles = RoleAccessGroups.Coordination)]
public class CoordinationController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(
        int naviresPage = 1, int cargoPage = 1, int incidentsPage = 1,
        CancellationToken cancellationToken = default)
    {
        var dto = await mediator.Send(
            new GetCoordinatorDashboardQuery(naviresPage, cargoPage, incidentsPage),
            cancellationToken);
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
