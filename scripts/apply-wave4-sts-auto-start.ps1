$ErrorActionPreference = 'Stop'

$commandPath = (Resolve-Path -LiteralPath '.\EscaleReport.Web\Application\Dispatch\Commands\AssignGantry\AssignGantryCommand.cs').Path
$command = [IO.File]::ReadAllText($commandPath)
$old = '    DateTime HeureDebut,'
$new = '    DateTime? HeureDebut,'
if (-not $command.Contains($old)) { throw 'HeureDebut AssignGantry introuvable.' }
[IO.File]::WriteAllText($commandPath, $command.Replace($old, $new), [Text.UTF8Encoding]::new($false))

$handlerPath = (Resolve-Path -LiteralPath '.\EscaleReport.Web\Application\Dispatch\Commands\AssignGantry\AssignGantryCommandHandler.cs').Path
$handler = [IO.File]::ReadAllText($handlerPath)
$old = '            HeureDebut = request.HeureDebut == default ? DateTime.UtcNow : request.HeureDebut,'
$new = '            HeureDebut = request.HeureDebut ?? DateTime.UtcNow,'
if (-not $handler.Contains($old)) { throw 'Affectation HeureDebut handler introuvable.' }
[IO.File]::WriteAllText($handlerPath, $handler.Replace($old, $new), [Text.UTF8Encoding]::new($false))

Write-Output 'Heure de début automatique rendue réellement optionnelle.'
