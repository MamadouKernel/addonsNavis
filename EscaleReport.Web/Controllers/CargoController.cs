using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Cargo.Commands.MarkRevisedLoadReceived;
using EscaleReport.Web.Application.Cargo.Commands.UpdateCargoConsommation;
using EscaleReport.Web.Application.Cargo.Queries.GetCargoDashboard;
using EscaleReport.Web.Application.Cargo.Queries.GetCargoDetail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Module Cargo Control (CDC §10).
[Authorize(Roles = RoleAccessGroups.Cargo)]
public class CargoController(ISender mediator, ICurrentUserService currentUser) : Controller
{
    public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
    {
        var rows = await mediator.Send(new GetCargoDashboardQuery(page), cancellationToken);
        return View(rows);
    }

    public async Task<IActionResult> Detail(Guid id, string? affichage, CancellationToken cancellationToken)
    {
        var detail = await mediator.Send(new GetCargoDetailQuery(id), cancellationToken);
        if (detail is null)
        {
            return NotFound();
        }

        var isAdmin = currentUser.IsInRole(Roles.Administrateur);
        ViewData["CargoViewMode"] = isAdmin && affichage is "planning" or "supervision"
            ? affichage
            : "operations";
        ViewData["IsCargoAdmin"] = isAdmin;
        return View(detail);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateConsommation(UpdateCargoConsommationCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Vérifiez les valeurs saisies (nombres positifs).";
            return RedirectToAction(nameof(Detail), new { id = command.EscaleId });
        }

        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Detail), new { id = command.EscaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkRevisedLoadReceived(Guid escaleId, string? observations, CancellationToken cancellationToken)
    {
        await mediator.Send(new MarkRevisedLoadReceivedCommand(escaleId, observations), cancellationToken);
        return RedirectToAction(nameof(Detail), new { id = escaleId });
    }
}
