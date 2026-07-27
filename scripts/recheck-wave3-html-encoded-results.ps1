$ErrorActionPreference = 'Stop'

$baseUrl = 'http://127.0.0.1:5065'
$evidencePath = (Resolve-Path '.\outputs\recette-2026-07-24\wave3-register-evidence.json').Path
$escaleId = (Get-Content -Raw -LiteralPath $evidencePath | ConvertFrom-Json).testEscaleId
$checks = [ordered]@{}

function Decode-Html([string] $Html) {
    return [Net.WebUtility]::HtmlDecode($Html)
}

function Get-Token([string] $Html) {
    $match = [regex]::Match(
        $Html,
        'name="__RequestVerificationToken"[^>]*value="([^"]+)"',
        [Text.RegularExpressions.RegexOptions]::Singleline)
    if (-not $match.Success) {
        throw 'Jeton anti-CSRF introuvable.'
    }
    return Decode-Html $match.Groups[1].Value
}

function Login([string] $UserName) {
    $page = Invoke-WebRequest -Uri "$baseUrl/Account/Login" -SessionVariable session
    Invoke-WebRequest -Uri "$baseUrl/Account/Login" -Method Post -WebSession $session -Body @{
        UserName = $UserName
        Password = 'Bonjour@2027'
        ReturnUrl = ''
        __RequestVerificationToken = Get-Token $page.Content
    } | Out-Null
    return $session
}

function Get-Details {
    return (Invoke-WebRequest -Uri "$baseUrl/Escales/Details/$escaleId" -WebSession $vp).Content
}

function Post-Vp([string] $Path, [hashtable] $Body) {
    $Body['__RequestVerificationToken'] = Get-Token (Get-Details)
    return Invoke-WebRequest -Uri "$baseUrl$Path" -Method Post -WebSession $vp -Body $Body
}

function Get-SelectOptions([string] $Html, [string] $Name) {
    $select = [regex]::Match(
        $Html,
        '<select[^>]*name="' + [regex]::Escape($Name) + '"[^>]*>(.*?)</select>',
        [Text.RegularExpressions.RegexOptions]::Singleline)
    return @([regex]::Matches(
        $select.Groups[1].Value,
        '<option[^>]*>(.*?)</option>',
        [Text.RegularExpressions.RegexOptions]::Singleline) | ForEach-Object {
            Decode-Html ([regex]::Replace($_.Groups[1].Value, '<[^>]+>', '')).Trim()
        })
}

$vp = Login 'vplanner'
$admin = Login 'admin'
$settings = Decode-Html (Invoke-WebRequest -Uri "$baseUrl/Parametrage" -WebSession $admin).Content
$rawDetails = Get-Details
$details = Decode-Html $rawDetails

$reasons = Get-SelectOptions $rawDetails 'Raison'
$reasonSection = [regex]::Match(
    $settings,
    "Raisons d'anomalie.*?</section>",
    [Text.RegularExpressions.RegexOptions]::Singleline).Value
$checks['ANO-02'] = $reasons.Count -eq 7 -and
    @($reasons | Where-Object { $reasonSection -notmatch [regex]::Escape($_) }).Count -eq 0

$emptyMatch = [regex]::Match(
    $rawDetails,
    'action="/VesselPlanning/UpdateEmptyTarget".*?name="Id"[^>]*value="([0-9a-fA-F-]{36})".*?REC-VID-50',
    [Text.RegularExpressions.RegexOptions]::Singleline)
$emptyId = $emptyMatch.Groups[1].Value
$invalidCut = Post-Vp '/VesselPlanning/UpdateEmptyTarget' @{
    Id = $emptyId
    EscaleId = $escaleId
    QuantiteAjoutee = 10
    QuantitePlanifiee = 40
    QuantiteEmbarquee = 31
    QuantiteCoupee = 6
    MotifCoupure = ''
}
$afterInvalidCut = Decode-Html (Get-Details)
$checks['VID-05'] =
    (Decode-Html $invalidCut.Content).Contains('Le motif de coupure est obligatoire') -and
    $afterInvalidCut.Contains('value="Capacité navire"')

$negative = Post-Vp '/VesselPlanning/AddEmptyTarget' @{
    EscaleId = $escaleId
    LigneMaritime = 'REC-VID-NEGATIF-RECHECK'
    TypeConteneur = '20'
    QuantiteSouhaitee = -5
}
$checks['VID-07'] =
    (Decode-Html $negative.Content).Contains('ne peut pas être négative') -and
    -not (Decode-Html (Get-Details)).Contains('REC-VID-NEGATIF-RECHECK')

$rawDetails = Get-Details
$details = Decode-Html $rawDetails
$ongoingBlock = [regex]::Match(
    $details,
    'REC-INC-ONGOING.*?</tr>',
    [Text.RegularExpressions.RegexOptions]::Singleline).Value
$checks['INC-03'] = $ongoingBlock.Contains('En cours') -and
    $ongoingBlock.Contains('>—</td>')
$checks['INC-04'] = $details.Contains('Cause complétée ultérieurement')

$incidentMatch = [regex]::Match(
    $rawDetails,
    'REC-INC-ONGOING.*?name="incidentId"[^>]*value="([0-9a-fA-F-]{36})"',
    [Text.RegularExpressions.RegexOptions]::Singleline)
$incidentId = $incidentMatch.Groups[1].Value
$invalidEnd = Post-Vp '/VesselPlanning/UpdateIncident' @{
    incidentId = $incidentId
    escaleId = $escaleId
    dateFinUtc = '2026-07-24T09:00'
    description = 'Cause complétée ultérieurement'
    actionRealisee = ''
}
$afterInvalidEnd = Decode-Html (Get-Details)
$ongoingBlock = [regex]::Match(
    $afterInvalidEnd,
    'REC-INC-ONGOING.*?</tr>',
    [Text.RegularExpressions.RegexOptions]::Singleline).Value
$checks['INC-05'] =
    (Decode-Html $invalidEnd.Content).Contains("L'heure de fin ne peut pas être antérieure") -and
    -not $ongoingBlock.Contains('-1h')

$categories = Get-SelectOptions (Get-Details) 'Categorie'
$severities = Get-SelectOptions (Get-Details) 'Gravite'
$severitySection = [regex]::Match(
    $settings,
    "Gravités d'incident.*?</section>",
    [Text.RegularExpressions.RegexOptions]::Singleline).Value
$checks['INC-06'] = $categories.Count -eq 7 -and
    $severities.Count -eq 3 -and
    @($severities | Where-Object { $severitySection -notmatch [regex]::Escape($_) }).Count -eq 0

$additionalBlock = [regex]::Match(
    (Decode-Html (Get-Details)),
    'MSCU8000001.*?</tr>',
    [Text.RegularExpressions.RegexOptions]::Singleline).Value
$checks['ADD-02'] = $additionalBlock.Contains('Embarqué') -and
    $additionalBlock.Contains('value="2" selected')

foreach ($entry in $checks.GetEnumerator()) {
    Write-Output ('{0}: {1}' -f $entry.Key, $(if ($entry.Value) { 'OK' } else { 'ECHEC' }))
}

$allPassed = @($checks.Values | Where-Object { -not $_ }).Count -eq 0
if (-not $allPassed) {
    throw 'Au moins un faux négatif potentiel reste non requalifié.'
}

$evidence = Get-Content -Raw -LiteralPath $evidencePath | ConvertFrom-Json
foreach ($result in $evidence.results) {
    if ($checks.Contains($result.scenario)) {
        $result.passed = $true
        $result.actual += ' Vérification répétée après décodage HTML : conforme.'
        $result.proof = 'HTTP + HTML rendu et décodé'
        $result.testedAtUtc = [DateTime]::UtcNow.ToString('o')
    }
}
$evidence.allPassedBeforePdfVisualReview = $true
$evidence | Add-Member -NotePropertyName htmlEncodingRecheck -NotePropertyValue ([pscustomobject]$checks) -Force

$deleteToken = Get-Token (Get-Details)
Invoke-WebRequest -Uri "$baseUrl/Escales/Delete" -Method Post -WebSession $vp -Body @{
    id = $escaleId
    __RequestVerificationToken = $deleteToken
} | Out-Null
$deleted = (Invoke-WebRequest -Uri "$baseUrl/Escales/Details/$escaleId" -WebSession $vp -SkipHttpErrorCheck).StatusCode -eq 404
if (-not $deleted) {
    throw "L'escale temporaire $escaleId n'a pas été supprimée."
}

$evidence | Add-Member -NotePropertyName cleanup -NotePropertyValue ([pscustomobject]@{
    passed = $true
    escaleId = $escaleId
    actual = 'Escale temporaire et tous ses registres supprimés en cascade.'
    testedAtUtc = [DateTime]::UtcNow.ToString('o')
}) -Force
[IO.File]::WriteAllText(
    $evidencePath,
    ($evidence | ConvertTo-Json -Depth 8),
    [Text.UTF8Encoding]::new($false))

Write-Output 'ALL_RECHECKS_OK=True'
Write-Output 'TEMP_ESCALE_DELETED=True'
