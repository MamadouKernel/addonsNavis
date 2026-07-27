using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Application.Settings.Commands.AddGantry;
using EscaleReport.Web.Application.Settings.Commands.AddReferenceValue;
using EscaleReport.Web.Application.Settings.Commands.RemoveAlertThreshold;
using EscaleReport.Web.Application.Settings.Commands.RemoveGantry;
using EscaleReport.Web.Application.Settings.Commands.ToggleCoordinatorModuleVisibility;
using EscaleReport.Web.Application.Settings.Commands.ToggleReferenceValueActive;
using EscaleReport.Web.Application.Settings.Commands.UpdateGeneralSettings;
using EscaleReport.Web.Application.Settings.Commands.UpsertAlertThreshold;
using EscaleReport.Web.Application.Settings.Commands.UpsertEmailTemplate;
using EscaleReport.Web.Application.Settings.Queries.GetParametrage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Module Paramétrage de la solution (CDC §15.1).
[Authorize(Roles = RoleAccessGroups.Administration)]
public class ParametrageController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(new GetParametrageQuery(), cancellationToken);
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateGeneralSettings(UpdateGeneralSettingsCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReferenceValue(AddReferenceValueCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleReferenceValueActive(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new ToggleReferenceValueActiveCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddGantry(AddGantryCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveGantry(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveGantryCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpsertEmailTemplate(UpsertEmailTemplateCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpsertAlertThreshold(UpsertAlertThresholdCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveAlertThreshold(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveAlertThresholdCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleCoordinatorModuleVisibility(string cle, CancellationToken cancellationToken)
    {
        await mediator.Send(new ToggleCoordinatorModuleVisibilityCommand(cle), cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
