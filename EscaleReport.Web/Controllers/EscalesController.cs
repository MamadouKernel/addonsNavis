using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Escales.Commands.ChangeEscaleStatutOperations;
using EscaleReport.Web.Application.Escales.Commands.ChangeEscaleStatutPlanification;
using EscaleReport.Web.Application.Escales.Commands.CreateEscale;
using EscaleReport.Web.Application.Escales.Commands.LogReportEmail;
using EscaleReport.Web.Application.Escales.Queries.GenerateEscaleExcelExport;
using EscaleReport.Web.Application.Escales.Queries.GenerateEscaleReport;
using EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;
using EscaleReport.Web.Application.Escales.Queries.GetEscales;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

[Authorize]
public class EscalesController(ISender mediator) : Controller
{
    // Tableau de bord des escales (CDC §4.1).
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var escales = await mediator.Send(new GetEscalesQuery(), cancellationToken);
        return View(escales);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEscaleCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(command);
        }

        try
        {
            await mediator.Send(command, cancellationToken);
        }
        catch (PossibleDuplicateEscaleException ex)
        {
            ViewBag.DuplicateWarning = ex.Message;
            return View(command with { ConfirmerDoublon = true });
        }

        return RedirectToAction(nameof(Index));
    }

    // Détail escale + module Vessel Planning (anomalies conteneurs, CDC §5.1).
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var detail = await mediator.Send(new GetEscaleDetailQuery(id), cancellationToken);
        if (detail is null)
        {
            return NotFound();
        }

        return View(detail);
    }

    // CDC §4.3 : passage "Terminées" réservé aux utilisateurs habilités (contrôlé côté handler).
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatutOperations(ChangeEscaleStatutOperationsCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Details), new { id = command.EscaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatutPlanification(ChangeEscaleStatutPlanificationCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Details), new { id = command.EscaleId });
    }

    // Génération directe par l'application (QuestPDF), pas via l'impression navigateur — CDC §14.3.
    public async Task<IActionResult> Report(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GenerateEscaleReportQuery(id), cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return File(result.PdfBytes, "application/pdf", result.FileName);
    }

    // CDC §14.4 "Export Excel".
    public async Task<IActionResult> ExportExcel(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GenerateEscaleExcelExportQuery(id), cancellationToken);
        if (result is null)
        {
            return NotFound();
        }

        return File(result.ExcelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.FileName);
    }

    // CDC §14.5 : ouvre un brouillon pré-rempli dans le client de messagerie de l'utilisateur ;
    // le PDF reste à joindre manuellement avant l'envoi réel (le mailto: ne permet pas de
    // pièce jointe). L'intention d'envoi est journalisée avant la redirection.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendReportEmail(Guid id, string recipients, CancellationToken cancellationToken)
    {
        var mailto = await mediator.Send(new LogReportEmailCommand(id, recipients), cancellationToken);
        if (mailto is null)
        {
            return NotFound();
        }

        return Redirect(mailto);
    }
}
