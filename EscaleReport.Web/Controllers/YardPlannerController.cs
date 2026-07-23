using EscaleReport.Web.Application.YardPlanning.Commands.AddHousekeepingTask;
using EscaleReport.Web.Application.YardPlanning.Commands.AddTransfertOut;
using EscaleReport.Web.Application.YardPlanning.Commands.AddVesselYardPlan;
using EscaleReport.Web.Application.YardPlanning.Commands.ChangeHousekeepingTaskStatus;
using EscaleReport.Web.Application.YardPlanning.Commands.EndTransfertOut;
using EscaleReport.Web.Application.YardPlanning.Commands.UpdateVesselYardPlan;
using EscaleReport.Web.Application.YardPlanning.Queries.GetYardPlannerDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Module Yard Planner (CDC §11).
[Authorize]
public class YardPlannerController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(
        int naviresPage = 1, int plansPage = 1, int transfertsPage = 1, int housekeepingPage = 1,
        CancellationToken cancellationToken = default)
    {
        var dto = await mediator.Send(
            new GetYardPlannerDashboardQuery(naviresPage, plansPage, transfertsPage, housekeepingPage),
            cancellationToken);
        return View(dto);
    }

    // ---------- Plans navire et zones de débarquement (§11.2) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddVesselYardPlan(AddVesselYardPlanCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVesselYardPlan(UpdateVesselYardPlanCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    // ---------- Transferts Out (§11.3) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTransfertOut(AddTransfertOutCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EndTransfertOut(Guid transfertId, CancellationToken cancellationToken)
    {
        await mediator.Send(new EndTransfertOutCommand(transfertId), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    // ---------- Housekeeping (§11.4) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddHousekeepingTask(AddHousekeepingTaskCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeHousekeepingTaskStatus(ChangeHousekeepingTaskStatusCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
