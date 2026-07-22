using EscaleReport.Web.Application.Dispatch.Commands.AddGateTruckIssue;
using EscaleReport.Web.Application.Dispatch.Commands.AddRopnEntry;
using EscaleReport.Web.Application.Dispatch.Commands.AddRtgClash;
using EscaleReport.Web.Application.Dispatch.Commands.AddRtgPanne;
using EscaleReport.Web.Application.Dispatch.Commands.AddStsIncident;
using EscaleReport.Web.Application.Dispatch.Commands.AddStsPointeur;
using EscaleReport.Web.Application.Dispatch.Commands.AssignGantry;
using EscaleReport.Web.Application.Dispatch.Commands.AssignTt;
using EscaleReport.Web.Application.Dispatch.Commands.ChangeGantryStatus;
using EscaleReport.Web.Application.Dispatch.Commands.CloseGateTruckIssue;
using EscaleReport.Web.Application.Dispatch.Commands.CloseRtgClash;
using EscaleReport.Web.Application.Dispatch.Commands.CloseRtgPanne;
using EscaleReport.Web.Application.Dispatch.Commands.CloseStsIncident;
using EscaleReport.Web.Application.Dispatch.Commands.EndGantryAssignment;
using EscaleReport.Web.Application.Dispatch.Commands.EndStsPointeur;
using EscaleReport.Web.Application.Dispatch.Commands.ResolveRopnEntry;
using EscaleReport.Web.Application.Dispatch.Commands.UpdateRtgEffectif;
using EscaleReport.Web.Application.Dispatch.Commands.UpdateTtEffectif;
using EscaleReport.Web.Application.Dispatch.Queries.GetDispatchRtg;
using EscaleReport.Web.Application.Dispatch.Queries.GetDispatchSts;
using EscaleReport.Web.Application.Dispatch.Queries.GetDispatchTt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

[Authorize]
public class DispatchController(ISender mediator) : Controller
{
    // Tableau de bord Dispatch STS (CDC §6).
    public async Task<IActionResult> Sts(CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(new GetDispatchStsQuery(), cancellationToken);
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignGantry(AssignGantryCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Sélectionnez un portique disponible et un navire.";
            return RedirectToAction(nameof(Sts));
        }

        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EndAssignment(Guid assignmentId, CancellationToken cancellationToken)
    {
        await mediator.Send(new EndGantryAssignmentCommand(assignmentId), cancellationToken);
        return RedirectToAction(nameof(Sts));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeGantryStatus(ChangeGantryStatusCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }

    // ---------- Incidents STS (§6.3) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddStsIncident(AddStsIncidentCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseStsIncident(Guid incidentId, string? conditionsReprise, CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseStsIncidentCommand(incidentId, conditionsReprise), cancellationToken);
        return RedirectToAction(nameof(Sts));
    }

    // ---------- Pointeurs (§6.4) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddStsPointeur(AddStsPointeurCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EndStsPointeur(Guid pointeurId, CancellationToken cancellationToken)
    {
        await mediator.Send(new EndStsPointeurCommand(pointeurId), cancellationToken);
        return RedirectToAction(nameof(Sts));
    }

    // ---------- ROPN (§6.5) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRopnEntry(AddRopnEntryCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolveRopnEntry(Guid ropnEntryId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new ResolveRopnEntryCommand(ropnEntryId, actionRealisee), cancellationToken);
        return RedirectToAction(nameof(Sts));
    }

    // Tableau de bord Dispatch TT (CDC §7).
    public async Task<IActionResult> Tt(CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(new GetDispatchTtQuery(), cancellationToken);
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateTtEffectif(UpdateTtEffectifCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Vérifiez les compteurs saisis (désignés ≤ total, retirés ≤ désignés).";
            return RedirectToAction(nameof(Tt));
        }

        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Tt));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignTt(AssignTtCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Sélectionnez un navire et vérifiez les valeurs saisies.";
            return RedirectToAction(nameof(Tt));
        }

        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Tt));
    }

    // Tableau de bord Dispatch RTG (CDC §8).
    public async Task<IActionResult> Rtg(CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(new GetDispatchRtgQuery(), cancellationToken);
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRtgEffectif(UpdateRtgEffectifCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Vérifiez les compteurs saisis (la somme ne doit pas dépasser le total du parc).";
            return RedirectToAction(nameof(Rtg));
        }

        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Rtg));
    }

    // ---------- Pannes RTG (§8.3) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRtgPanne(AddRtgPanneCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Rtg));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseRtgPanne(Guid panneId, string? commentaireReprise, CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseRtgPanneCommand(panneId, commentaireReprise), cancellationToken);
        return RedirectToAction(nameof(Rtg));
    }

    // ---------- Clashs (§8.4) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRtgClash(AddRtgClashCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Rtg));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseRtgClash(Guid clashId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseRtgClashCommand(clashId, actionRealisee), cancellationToken);
        return RedirectToAction(nameof(Rtg));
    }

    // ---------- Problèmes camions Gate (§8.5) ----------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddGateTruckIssue(AddGateTruckIssueCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Rtg));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseGateTruckIssue(Guid issueId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseGateTruckIssueCommand(issueId, actionRealisee), cancellationToken);
        return RedirectToAction(nameof(Rtg));
    }
}
