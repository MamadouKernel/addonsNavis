$ErrorActionPreference = 'Stop'

$path = (Resolve-Path -LiteralPath '.\scripts\run-wave4-sts-tests.ps1').Path
$content = [IO.File]::ReadAllText($path).Replace("`r`n", "`n")

$marker = @'
function Get-AssignmentArea([string] $Html) {
'@
$helper = @'
function Get-IncidentEntryBlock([string] $Html, [string] $UniqueMarker) {
    $index = $Html.IndexOf($UniqueMarker, [StringComparison]::Ordinal)
    if ($index -lt 0) { return '' }
    $editRowStart = $Html.LastIndexOf('<tr', $index, [StringComparison]::Ordinal)
    $displayRowStart = $Html.LastIndexOf('<tr', $editRowStart - 1, [StringComparison]::Ordinal)
    $editRowEnd = $Html.IndexOf('</tr>', $index, [StringComparison]::Ordinal)
    if ($displayRowStart -lt 0 -or $editRowEnd -lt 0) { return '' }
    return $Html.Substring($displayRowStart, ($editRowEnd + 5) - $displayRowStart)
}

function Get-AssignmentArea([string] $Html) {
'@
if (-not $content.Contains($marker)) { throw 'Point insertion helper incident introuvable.' }
$content = $content.Replace($marker, $helper)

$content = $content.Replace(
    '$incidentRow = Get-RowContaining $html ''Panne mécanique''',
    '$incidentRow = Get-IncidentEntryBlock $html ''RECETTE PANNE INLINE''')

$content = $content.Replace(
    '$incidentRow = Get-RowContaining $html ''Panne automate''
    $incidentId = Get-GuidAfter $html ''Panne automate'' ''IncidentId''',
    '$incidentRow = Get-IncidentEntryBlock $html ''RECETTE INCIDENT STS4''
    $incidentId = ([regex]::Match($incidentRow, ''name="IncidentId" value="([0-9a-fA-F-]{36})"'')).Groups[1].Value')

$content = $content.Replace(
    '$incidentRow = Get-RowContaining $html ''Panne électrique''',
    '$incidentRow = Get-IncidentEntryBlock $html ''RECETTE INCIDENT MODIFIE''')

$content = $content.Replace(
    '$autoStart.ToUniversalTime() -ge $beforeAssign.AddMinutes(-1) -and
        $autoStart.ToUniversalTime() -le $afterAssign.AddMinutes(1)',
    '[DateTime]::SpecifyKind($autoStart, [DateTimeKind]::Utc) -ge $beforeAssign.AddMinutes(-1) -and
        [DateTime]::SpecifyKind($autoStart, [DateTimeKind]::Utc) -le $afterAssign.AddMinutes(1)')

[IO.File]::WriteAllText($path, $content, [Text.UTF8Encoding]::new($false))
Write-Output 'Harnais STS fiabilisé pour les blocs incidents.'
