using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Application.Reporting.Commands.ConfirmPriseDeConnaissance;
using EscaleReport.Web.Application.Reporting.Commands.UpsertEscalePlanificationNote;
using EscaleReport.Web.Application.Reporting.Commands.UpsertShiftHandoverNote;
using EscaleReport.Web.Application.Reporting.Commands.ValidateShiftReport;
using EscaleReport.Web.Application.Reporting.Queries.GenerateShiftReport;
using EscaleReport.Web.Application.Reporting.Queries.GetShiftReport;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Module Rapport de fin de shift (CDC §14.1).
[Authorize(Roles = RoleAccessGroups.Reporting)]
public class ReportingController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(DateOnly? date, string? shift, Guid? escaleId, CancellationToken cancellationToken)
    {
        var selectedDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var dto = await mediator.Send(new GetShiftReportQuery(selectedDate, shift, escaleId), cancellationToken);
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveHandoverNote(
        DateOnly date, string? shift, Guid? escaleId, string? actionsEnCours, string? pointsATransmettre, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpsertShiftHandoverNoteCommand(date, shift, actionsEnCours, pointsATransmettre), cancellationToken);
        return RedirectToAction(nameof(Index), new { date, shift, escaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SavePlanificationNote(
        Guid escaleId, string? commentaire, DateOnly date, string? shift, Guid? currentEscaleId, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpsertEscalePlanificationNoteCommand(escaleId, commentaire), cancellationToken);
        return RedirectToAction(nameof(Index), new { date, shift, escaleId = currentEscaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ValidateReport(
        DateOnly date, string? shift, Guid? escaleId, string? commentaireValidation, CancellationToken cancellationToken)
    {
        await mediator.Send(new ValidateShiftReportCommand(date, shift, commentaireValidation), cancellationToken);
        return RedirectToAction(nameof(Index), new { date, shift, escaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmPriseDeConnaissance(
        DateOnly date, string? shift, Guid? escaleId, CancellationToken cancellationToken)
    {
        await mediator.Send(new ConfirmPriseDeConnaissanceCommand(date, shift), cancellationToken);
        return RedirectToAction(nameof(Index), new { date, shift, escaleId });
    }

    public async Task<IActionResult> GeneratePdf(DateOnly date, string? shift, Guid? escaleId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GenerateShiftReportQuery(date, shift, escaleId), cancellationToken);
        return File(result.PdfBytes, "application/pdf", result.FileName);
    }
}
