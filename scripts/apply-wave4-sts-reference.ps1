$ErrorActionPreference = 'Stop'

$path = (Resolve-Path -LiteralPath '.\EscaleReport.Web\Domain\Common\ReferenceValue.cs').Path
$content = [IO.File]::ReadAllText($path)

if (-not $content.Contains('public const string StsVesselIncidentType')) {
    $content = $content.Replace(
        '    public const string StsIncidentType = "StsIncidentType";',
        "    public const string StsIncidentType = `"StsIncidentType`";`r`n    public const string StsVesselIncidentType = `"StsVesselIncidentType`";")
}

$content = $content.Replace(
    '        [StsIncidentType] = "Types de panne STS",',
    "        [StsIncidentType] = `"Types de panne portique STS`",`r`n        [StsVesselIncidentType] = `"Types d'incident navire STS`",")

[IO.File]::WriteAllText($path, $content, [Text.UTF8Encoding]::new($false))
Write-Output 'Listes de référence STS mises à jour.'
