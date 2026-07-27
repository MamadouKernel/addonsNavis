$ErrorActionPreference = 'Stop'

function Update-ExactFile {
    param(
        [Parameter(Mandatory)] [string] $Path,
        [Parameter(Mandatory)] [hashtable[]] $Replacements
    )

    $resolved = (Resolve-Path -LiteralPath $Path).Path
    $text = [IO.File]::ReadAllText($resolved).Replace("`r`n", "`n")

    foreach ($replacement in $Replacements) {
        $old = ([string]$replacement.Old).Replace("`r`n", "`n")
        $new = ([string]$replacement.New).Replace("`r`n", "`n")
        if (-not $text.Contains($old)) {
            throw "Bloc attendu introuvable dans $Path : $($old.Substring(0, [Math]::Min(80, $old.Length)))"
        }
        $text = $text.Replace($old, $new)
    }

    [IO.File]::WriteAllText($resolved, $text.Replace("`n", [Environment]::NewLine), [Text.UTF8Encoding]::new($false))
}

$controller = 'EscaleReport.Web\Controllers\VesselPlanningController.cs'
Update-ExactFile -Path $controller -Replacements @(
    @{
        Old = @'
using EscaleReport.Web.Application.VesselPlanning.Commands.AddOperationalIncident;
using EscaleReport.Web.Application.VesselPlanning.Commands.ResolveContainerAnomaly;
'@
        New = @'
using EscaleReport.Web.Application.VesselPlanning.Commands.AddOperationalIncident;
using EscaleReport.Web.Application.VesselPlanning.Commands.DeleteContainerAnomaly;
using EscaleReport.Web.Application.VesselPlanning.Commands.ResolveContainerAnomaly;
'@
    },
    @{
        Old = @'
using EscaleReport.Web.Application.VesselPlanning.Commands.SetAdditionalContainerDecision;
using EscaleReport.Web.Application.VesselPlanning.Commands.UpdateDangerousContainerStatus;
using EscaleReport.Web.Application.VesselPlanning.Commands.UpdateEmptyContainerTarget;
'@
        New = @'
using EscaleReport.Web.Application.VesselPlanning.Commands.SetAdditionalContainerDecision;
using EscaleReport.Web.Application.VesselPlanning.Commands.UpdateContainerAnomalyPosition;
using EscaleReport.Web.Application.VesselPlanning.Commands.UpdateDangerousContainerStatus;
using EscaleReport.Web.Application.VesselPlanning.Commands.UpdateEmptyContainerTarget;
using EscaleReport.Web.Application.VesselPlanning.Commands.UpdateOperationalIncident;
'@
    },
    @{
        Old = @'
        await mediator.Send(command, cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolveAnomaly
'@
        New = @'
        foreach (var numero in SplitContainerNumbers(command.NumeroConteneur))
        {
            await mediator.Send(command with { NumeroConteneur = numero }, cancellationToken);
        }
        return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolveAnomaly
'@
    },
    @{
        Old = @'
    public async Task<IActionResult> ResolveAnomaly(Guid anomalyId, Guid escaleId, CancellationToken cancellationToken)
    {
        await mediator.Send(new ResolveContainerAnomalyCommand(anomalyId), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    // ---------- Conteneurs vides à embarquer (§5.2) ----------
'@
        New = @'
    public async Task<IActionResult> ResolveAnomaly(Guid anomalyId, Guid escaleId, CancellationToken cancellationToken)
    {
        await mediator.Send(new ResolveContainerAnomalyCommand(anomalyId), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAnomalyPosition(
        Guid anomalyId, Guid escaleId, string? position, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateContainerAnomalyPositionCommand(anomalyId, position), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAnomaly(Guid anomalyId, Guid escaleId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteContainerAnomalyCommand(anomalyId), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    // ---------- Conteneurs vides à embarquer (§5.2) ----------
'@
    },
    @{
        Old = @'
    public async Task<IActionResult> AddEmptyTarget(AddEmptyContainerTargetCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
'@
        New = @'
    public async Task<IActionResult> AddEmptyTarget(AddEmptyContainerTargetCommand command, CancellationToken cancellationToken)
    {
        if (command.QuantiteSouhaitee < 0)
        {
            TempData["Error"] = "La quantité souhaitée ne peut pas être négative.";
            return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
        }

        await mediator.Send(command, cancellationToken);
'@
    },
    @{
        Old = @'
    public async Task<IActionResult> UpdateEmptyTarget(UpdateEmptyContainerTargetCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
'@
        New = @'
    public async Task<IActionResult> UpdateEmptyTarget(UpdateEmptyContainerTargetCommand command, CancellationToken cancellationToken)
    {
        if (command.QuantiteAjoutee < 0 || command.QuantitePlanifiee < 0 ||
            command.QuantiteEmbarquee < 0 || command.QuantiteCoupee < 0)
        {
            TempData["Error"] = "Les quantités ne peuvent pas être négatives.";
            return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
        }

        if (command.QuantiteCoupee > 0 && string.IsNullOrWhiteSpace(command.MotifCoupure))
        {
            TempData["Error"] = "Le motif de coupure est obligatoire lorsqu'une quantité est coupée.";
            return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
        }

        await mediator.Send(command, cancellationToken);
'@
    },
    @{
        Old = @'
    public async Task<IActionResult> AddIncident(AddOperationalIncidentCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
'@
        New = @'
    public async Task<IActionResult> AddIncident(AddOperationalIncidentCommand command, CancellationToken cancellationToken)
    {
        if (command.DateFinUtc.HasValue && command.DateFinUtc.Value < command.DateDebutUtc)
        {
            TempData["Error"] = "L'heure de fin ne peut pas être antérieure à l'heure de début.";
            return RedirectToAction("Details", "Escales", new { id = command.EscaleId });
        }

        await mediator.Send(command, cancellationToken);
'@
    },
    @{
        Old = @'
    public async Task<IActionResult> ResolveIncident(Guid incidentId, Guid escaleId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new ResolveOperationalIncidentCommand(incidentId, actionRealisee), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    // ---------- Conteneurs additionnels (§5.4) ----------
'@
        New = @'
    public async Task<IActionResult> ResolveIncident(Guid incidentId, Guid escaleId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new ResolveOperationalIncidentCommand(incidentId, actionRealisee), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateIncident(
        Guid incidentId, Guid escaleId, DateTime? dateFinUtc, string? description,
        string? actionRealisee, CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(
            new UpdateOperationalIncidentCommand(incidentId, dateFinUtc, description, actionRealisee),
            cancellationToken);
        if (!updated)
        {
            TempData["Error"] = "L'heure de fin ne peut pas être antérieure à l'heure de début.";
        }

        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    // ---------- Conteneurs additionnels (§5.4) ----------
'@
    },
    @{
        Old = @'
    public async Task<IActionResult> AddAdditional(AddAdditionalContainerCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
'@
        New = @'
    public async Task<IActionResult> AddAdditional(AddAdditionalContainerCommand command, CancellationToken cancellationToken)
    {
        foreach (var numero in SplitContainerNumbers(command.NumeroConteneur))
        {
            await mediator.Send(command with { NumeroConteneur = numero }, cancellationToken);
        }
'@
    },
    @{
        Old = @'
    public async Task<IActionResult> AddDangerous(AddDangerousContainerCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
'@
        New = @'
    public async Task<IActionResult> AddDangerous(AddDangerousContainerCommand command, CancellationToken cancellationToken)
    {
        foreach (var numero in SplitContainerNumbers(command.NumeroConteneur))
        {
            await mediator.Send(command with { NumeroConteneur = numero }, cancellationToken);
        }
'@
    },
    @{
        Old = @'
        await mediator.Send(new UpdateDangerousContainerStatusCommand(containerId, statutBadt, statutOperationnel), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }
}
'@
        New = @'
        await mediator.Send(new UpdateDangerousContainerStatusCommand(containerId, statutBadt, statutOperationnel), cancellationToken);
        return RedirectToAction("Details", "Escales", new { id = escaleId });
    }

    private static IEnumerable<string> SplitContainerNumbers(string value) =>
        (value ?? string.Empty)
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(number => !string.IsNullOrWhiteSpace(number))
            .Distinct(StringComparer.OrdinalIgnoreCase);
}
'@
    }
)

Write-Output 'VesselPlanningController.cs mis à jour.'
