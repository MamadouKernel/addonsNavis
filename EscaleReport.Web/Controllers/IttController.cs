using EscaleReport.Web.Application.Itt.Commands.AddIttEnginPanne;
using EscaleReport.Web.Application.Itt.Commands.AddIttTransfer;
using EscaleReport.Web.Application.Itt.Commands.AddIttTransferIncident;
using EscaleReport.Web.Application.Itt.Commands.CloseIttEnginPanne;
using EscaleReport.Web.Application.Itt.Commands.CloseIttTransferIncident;
using EscaleReport.Web.Application.Itt.Commands.UpdateIttEquipementEffectif;
using EscaleReport.Web.Application.Itt.Commands.UpdateIttTransfer;
using EscaleReport.Web.Application.Itt.Queries.GetIttDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Module ITT Controller (CDC §13).
[Authorize]
public class IttController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(
        int transfersPage = 1, int incidentsPage = 1, int pannesPage = 1,
        CancellationToken cancellationToken = default)
    {
        var dto = await mediator.Send(
            new GetIttDashboardQuery(transfersPage, incidentsPage, pannesPage),
            cancellationToken);
        return View(dto);
    }

    // ---------- Suivi des transferts (§13.1) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTransfer(AddIttTransferCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateTransfer(UpdateIttTransferCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    // ---------- Incidents de transfert (§13.2) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTransferIncident(AddIttTransferIncidentCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseTransferIncident(Guid incidentId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseIttTransferIncidentCommand(incidentId, actionRealisee), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    // ---------- Équipements ITT (§13.3) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateEquipementEffectif(UpdateIttEquipementEffectifCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    // ---------- Pannes des engins de transfert (§13.4) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEnginPanne(AddIttEnginPanneCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseEnginPanne(Guid panneId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseIttEnginPanneCommand(panneId, actionRealisee), cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
