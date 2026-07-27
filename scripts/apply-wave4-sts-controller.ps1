$ErrorActionPreference = 'Stop'

function Replace-Normalized {
    param([string] $Path, [string] $Old, [string] $New, [string] $Label)
    $resolved = (Resolve-Path -LiteralPath $Path).Path
    $content = [IO.File]::ReadAllText($resolved).Replace("`r`n", "`n")
    $oldNormalized = $Old.Replace("`r`n", "`n")
    $newNormalized = $New.Replace("`r`n", "`n")
    if (-not $content.Contains($oldNormalized)) {
        throw "Bloc introuvable : $Label"
    }
    [IO.File]::WriteAllText(
        $resolved,
        $content.Replace($oldNormalized, $newNormalized),
        [Text.UTF8Encoding]::new($false))
}

$path = '.\EscaleReport.Web\Controllers\DispatchController.cs'

Replace-Normalized $path `
@'
using EscaleReport.Web.Application.Dispatch.Commands.UpdateTtEffectif;
'@ `
@'
using EscaleReport.Web.Application.Dispatch.Commands.UpdateTtEffectif;
using EscaleReport.Web.Application.Dispatch.Commands.UpdateGantryAssignment;
using EscaleReport.Web.Application.Dispatch.Commands.UpdateStsIncident;
'@ `
    'using commandes STS'

Replace-Normalized $path `
@'
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
'@ `
@'
    public async Task<IActionResult> AssignGantry(
        AssignGantryCommand command,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Sélectionnez un portique disponible et un navire.";
            return RedirectToAction(nameof(Sts), new { date, shift });
        }

        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@ `
    'AssignGantry'

Replace-Normalized $path `
@'
    public async Task<IActionResult> EndAssignment(Guid assignmentId, CancellationToken cancellationToken)
    {
        await mediator.Send(new EndGantryAssignmentCommand(assignmentId), cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@ `
@'
    public async Task<IActionResult> EndAssignment(
        Guid assignmentId,
        DateTime? heureFin,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new EndGantryAssignmentCommand(assignmentId, heureFin), cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAssignment(
        UpdateGantryAssignmentCommand command,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@ `
    'EndAssignment et UpdateAssignment'

Replace-Normalized $path `
@'
    public async Task<IActionResult> ChangeGantryStatus(ChangeGantryStatusCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@ `
@'
    public async Task<IActionResult> ChangeGantryStatus(
        ChangeGantryStatusCommand command,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@ `
    'ChangeGantryStatus'

Replace-Normalized $path `
@'
    public async Task<IActionResult> AddStsIncident(AddStsIncidentCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@ `
@'
    public async Task<IActionResult> AddStsIncident(
        AddStsIncidentCommand command,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@ `
    'AddStsIncident'

Replace-Normalized $path `
@'
    public async Task<IActionResult> CloseStsIncident(Guid incidentId, string? conditionsReprise, CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseStsIncidentCommand(incidentId, conditionsReprise), cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@ `
@'
    public async Task<IActionResult> CloseStsIncident(
        Guid incidentId,
        string? conditionsReprise,
        DateTime? dateFinUtc,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseStsIncidentCommand(incidentId, conditionsReprise, dateFinUtc), cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStsIncident(
        UpdateStsIncidentCommand command,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@ `
    'Close et UpdateStsIncident'

Replace-Normalized $path `
@'
    public async Task<IActionResult> AddStsPointeur(AddStsPointeurCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@ `
@'
    public async Task<IActionResult> AddStsPointeur(
        AddStsPointeurCommand command,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@ `
    'AddStsPointeur'

Replace-Normalized $path `
@'
    public async Task<IActionResult> EndStsPointeur(Guid pointeurId, CancellationToken cancellationToken)
    {
        await mediator.Send(new EndStsPointeurCommand(pointeurId), cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@ `
@'
    public async Task<IActionResult> EndStsPointeur(
        Guid pointeurId,
        DateTime? heureFinUtc,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new EndStsPointeurCommand(pointeurId, heureFinUtc), cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@ `
    'EndStsPointeur'

Replace-Normalized $path `
@'
    public async Task<IActionResult> AddRopnEntry(AddRopnEntryCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@ `
@'
    public async Task<IActionResult> AddRopnEntry(
        AddRopnEntryCommand command,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@ `
    'AddRopnEntry'

Replace-Normalized $path `
@'
    public async Task<IActionResult> ResolveRopnEntry(Guid ropnEntryId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new ResolveRopnEntryCommand(ropnEntryId, actionRealisee), cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@ `
@'
    public async Task<IActionResult> ResolveRopnEntry(
        Guid ropnEntryId,
        string? actionRealisee,
        DateTime? dateFinUtc,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new ResolveRopnEntryCommand(ropnEntryId, actionRealisee, dateFinUtc), cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@ `
    'ResolveRopnEntry'

Write-Output 'Contrôleur STS vague 4 mis à jour.'
