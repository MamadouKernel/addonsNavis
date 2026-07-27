$ErrorActionPreference = 'Stop'

$sqlMigration = Get-ChildItem -LiteralPath '.\EscaleReport.Web\Infrastructure\Persistence\Migrations\SqlServer' `
    -Filter '*_AddStsWorkflowCorrections.cs' -File | Select-Object -First 1
$postgresMigration = Get-ChildItem -LiteralPath '.\EscaleReport.Web\Infrastructure\Persistence\Migrations\Postgres' `
    -Filter '*_AddStsWorkflowCorrections.cs' -File | Select-Object -First 1

if ($null -eq $sqlMigration -or $null -eq $postgresMigration) {
    throw 'Migration STS introuvable pour un des fournisseurs.'
}

function Set-DefaultSql([string] $Path, [string] $DefaultSql) {
    $content = [IO.File]::ReadAllText($Path).Replace("`r`n", "`n")
    $old = 'nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));'
    $new = "nullable: false,`n                defaultValueSql: `"$DefaultSql`");"
    if (-not $content.Contains($old)) {
        throw "Valeur par défaut générée introuvable dans $Path"
    }
    [IO.File]::WriteAllText($Path, $content.Replace($old, $new), [Text.UTF8Encoding]::new($false))
}

Set-DefaultSql $sqlMigration.FullName 'GETUTCDATE()'
Set-DefaultSql $postgresMigration.FullName 'CURRENT_TIMESTAMP'
Write-Output 'Valeurs par défaut des migrations STS sécurisées.'
