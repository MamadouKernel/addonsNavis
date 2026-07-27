$ErrorActionPreference = 'Stop'

$path = (Resolve-Path -LiteralPath '.\EscaleReport.Web\Controllers\DispatchController.cs').Path
$content = [IO.File]::ReadAllText($path).Replace("`r`n", "`n")

$content = $content.Replace(
    "using EscaleReport.Web.Application.Dispatch.Commands.UpdateGantryAssignment;`nusing EscaleReport.Web.Application.Dispatch.Commands.UpdateStsIncident;`nusing EscaleReport.Web.Application.Dispatch.Commands.UpdateGantryAssignment;`nusing EscaleReport.Web.Application.Dispatch.Commands.UpdateStsIncident;",
    "using EscaleReport.Web.Application.Dispatch.Commands.UpdateGantryAssignment;`nusing EscaleReport.Web.Application.Dispatch.Commands.UpdateStsIncident;")

$old = @'
    public async Task<IActionResult> AddStsPointeur(AddStsPointeurCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@
$new = @'
    public async Task<IActionResult> AddStsPointeur(
        AddStsPointeurCommand command,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@
if (-not $content.Contains($old)) { throw 'AddStsPointeur introuvable.' }
$content = $content.Replace($old, $new)

$old = @'
    public async Task<IActionResult> EndStsPointeur(Guid pointeurId, CancellationToken cancellationToken)
    {
        await mediator.Send(new EndStsPointeurCommand(pointeurId), cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@
$new = @'
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
'@
if (-not $content.Contains($old)) { throw 'EndStsPointeur introuvable.' }
$content = $content.Replace($old, $new)

$old = @'
    public async Task<IActionResult> AddRopnEntry(AddRopnEntryCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@
$new = @'
    public async Task<IActionResult> AddRopnEntry(
        AddRopnEntryCommand command,
        DateOnly? date,
        string? shift,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Sts), new { date, shift });
    }
'@
if (-not $content.Contains($old)) { throw 'AddRopnEntry introuvable.' }
$content = $content.Replace($old, $new)

$old = @'
    public async Task<IActionResult> ResolveRopnEntry(Guid ropnEntryId, string? actionRealisee, CancellationToken cancellationToken)
    {
        await mediator.Send(new ResolveRopnEntryCommand(ropnEntryId, actionRealisee), cancellationToken);
        return RedirectToAction(nameof(Sts));
    }
'@
$new = @'
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
'@
if (-not $content.Contains($old)) { throw 'ResolveRopnEntry introuvable.' }
$content = $content.Replace($old, $new)

[IO.File]::WriteAllText($path, $content, [Text.UTF8Encoding]::new($false))
Write-Output 'Contrôleur STS réparé et dédoublonné.'
