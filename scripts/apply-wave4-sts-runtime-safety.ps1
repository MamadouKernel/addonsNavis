$ErrorActionPreference = 'Stop'

$queryPath = (Resolve-Path -LiteralPath '.\EscaleReport.Web\Application\Dispatch\Queries\GetDispatchSts\GetDispatchStsQueryHandler.cs').Path
$query = [IO.File]::ReadAllText($queryPath).Replace("`r`n", "`n")
$projection = '                HeureFin = a.HeureFin,
                Duree = a.HeureFin.HasValue ? a.HeureFin.Value - a.HeureDebut : null,
                TacheOuZone = a.TacheOuZone,'
$safeProjection = '                HeureFin = a.HeureFin,
                TacheOuZone = a.TacheOuZone,'
if (-not $query.Contains($projection)) {
    throw 'Projection de durée affectation introuvable.'
}
$query = $query.Replace($projection, $safeProjection)
$marker = '            }).ToListAsync(cancellationToken);

        var escalesDisponibles'
$replacement = '            }).ToListAsync(cancellationToken);
        foreach (var assignment in assignments)
        {
            assignment.Duree = assignment.HeureFin.HasValue
                ? assignment.HeureFin.Value - assignment.HeureDebut
                : null;
        }

        var escalesDisponibles'
if (-not $query.Contains($marker)) {
    throw 'Point de calcul client de la durée introuvable.'
}
$query = $query.Replace($marker, $replacement)
[IO.File]::WriteAllText($queryPath, $query, [Text.UTF8Encoding]::new($false))

$sqlMigration = Get-ChildItem -LiteralPath '.\EscaleReport.Web\Infrastructure\Persistence\Migrations\SqlServer' `
    -Filter '*_AddStsWorkflowCorrections.cs' -File | Select-Object -Single
$postgresMigration = Get-ChildItem -LiteralPath '.\EscaleReport.Web\Infrastructure\Persistence\Migrations\Postgres' `
    -Filter '*_AddStsWorkflowCorrections.cs' -File | Select-Object -Single

$sql = [IO.File]::ReadAllText($sqlMigration.FullName)
$sql = $sql.Replace(
    'nullable: false,' + [Environment]::NewLine + '                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));',
    'nullable: false,' + [Environment]::NewLine + '                defaultValueSql: "GETUTCDATE()");')
[IO.File]::WriteAllText($sqlMigration.FullName, $sql, [Text.UTF8Encoding]::new($false))

$postgres = [IO.File]::ReadAllText($postgresMigration.FullName)
$postgres = $postgres.Replace(
    'nullable: false,' + [Environment]::NewLine + '                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));',
    'nullable: false,' + [Environment]::NewLine + '                defaultValueSql: "CURRENT_TIMESTAMP");')
[IO.File]::WriteAllText($postgresMigration.FullName, $postgres, [Text.UTF8Encoding]::new($false))

Write-Output 'Sécurité runtime STS et valeurs par défaut des migrations appliquées.'
