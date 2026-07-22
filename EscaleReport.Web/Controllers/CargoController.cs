using EscaleReport.Web.Application.Cargo.Commands.MarkRevisedLoadReceived;
using EscaleReport.Web.Application.Cargo.Commands.UpdateCargoConsommation;
using EscaleReport.Web.Application.Cargo.Queries.GetCargoDashboard;
using EscaleReport.Web.Application.Cargo.Queries.GetCargoDetail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Module Cargo Control (CDC §10).
[Authorize]
public class CargoController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var rows = await mediator.Send(new GetCargoDashboardQuery(), cancellationToken);
        return View(rows);
    }

    public async Task<IActionResult> Detail(Guid id, CancellationToken cancellationToken)
    {
        var detail = await mediator.Send(new GetCargoDetailQuery(id), cancellationToken);
        if (detail is null)
        {
            return NotFound();
        }

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
