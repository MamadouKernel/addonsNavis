using EscaleReport.Web.Application.VesselPlanning.Commands.AddAdditionalContainer;
using EscaleReport.Web.Application.VesselPlanning.Commands.AddContainerAnomaly;
using EscaleReport.Web.Application.VesselPlanning.Commands.AddDangerousContainer;
using EscaleReport.Web.Application.VesselPlanning.Commands.AddEmptyContainerTarget;
using EscaleReport.Web.Application.VesselPlanning.Commands.AddOperationalIncident;
using EscaleReport.Web.Application.VesselPlanning.Commands.ResolveContainerAnomaly;
using EscaleReport.Web.Application.VesselPlanning.Commands.ResolveOperationalIncident;
using EscaleReport.Web.Application.VesselPlanning.Commands.SetAdditionalContainerDecision;
using EscaleReport.Web.Application.VesselPlanning.Commands.UpdateDangerousContainerStatus;
using EscaleReport.Web.Application.VesselPlanning.Commands.UpdateEmptyContainerTarget;
using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

[Authorize]
public class VesselPlanningController(ISender mediator) : Controller
{
    // ---------- Conteneurs en anomalie (§5.1) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAnomaly(AddContainerAnomalyCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Formulaire invalide : vérifiez le numéro de conteneur et la raison.";
            return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
        }

        await mediator.Send(command, cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolveAnomaly(Guid anomalyId, Guid escaleId, CancellationToken cancellationToken)
    {
        await mediator.Send(new ResolveContainerAnomalyCommand(anomalyId), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    // ---------- Conteneurs vides à embarquer (§5.2) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEmptyTarget(AddEmptyContainerTargetCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateEmptyTarget(UpdateEmptyContainerTargetCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
    }

    // ---------- Incidents opérationnels (§5.3) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddIncident(AddOperationalIncidentCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolveIncident(Guid incidentId, Guid escaleId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new ResolveOperationalIncidentCommand(incidentId, actionRealisee), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    // ---------- Conteneurs additionnels (§5.4) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAdditional(AddAdditionalContainerCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetAdditionalDecision(Guid containerId, Guid escaleId, AdditionalContainerDecision decision, CancellationToken cancellationToken)
    {
        await mediator.Send(new SetAdditionalContainerDecisionCommand(containerId, decision), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    // ---------- Conteneurs dangereux (§5.5) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddDangerous(AddDangerousContainerCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDangerousStatus(
        Guid containerId, Guid escaleId, BadtStatus statutBadt, DangerousContainerStatus statutOperationnel,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateDangerousContainerStatusCommand(containerId, statutBadt, statutOperationnel), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }
}
