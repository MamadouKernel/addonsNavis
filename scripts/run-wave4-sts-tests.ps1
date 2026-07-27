$ErrorActionPreference = 'Stop'

$baseUrl = 'http://127.0.0.1:5066'
$selectedDate = '2026-07-24'
$selectedShift = 'Matin'
$stamp = Get-Date -Format 'HHmmss'
$escaleNameA = "RECETTE-STS4-A-$stamp"
$escaleNameB = "RECETTE-STS4-B-$stamp"
$pointerName = "POINTEUR-STS4-$stamp"
$ropnName = "ROPN-STS4-$stamp"
$evidencePath = Join-Path (Resolve-Path '.\outputs\recette-2026-07-24').Path 'wave4-sts-evidence.json'
$failureHtmlPath = 'C:\tmp\wave4-sts-failure.html'
$evidence = [Collections.Generic.List[object]]::new()
$allPassed = $true
$escaleIdA = $null
$escaleIdB = $null

function Get-AntiForgeryToken([string] $Html) {
    $match = [regex]::Match(
        $Html,
        'name="__RequestVerificationToken"[^>]*value="([^"]+)"',
        [Text.RegularExpressions.RegexOptions]::Singleline)
    if (-not $match.Success) {
        throw 'Jeton anti-CSRF introuvable.'
    }
    return [Net.WebUtility]::HtmlDecode($match.Groups[1].Value)
}

function Record-Evidence(
    [string] $Scenario,
    [bool] $Passed,
    [string] $Actual,
    [string] $Proof = 'HTTP + HTML rendu') {
    $script:evidence.Add([pscustomobject]@{
        scenario = $Scenario
        passed = $Passed
        actual = $Actual
        proof = $Proof
        testedAtUtc = [DateTime]::UtcNow.ToString('o')
    })
    if (-not $Passed) {
        $script:allPassed = $false
    }
    Write-Output ('{0}: {1} - {2}' -f $Scenario, $(if ($Passed) { 'OK' } else { 'ECHEC' }), $Actual)
}

function New-AuthenticatedSession([string] $UserName) {
    $login = Invoke-WebRequest -Uri "$baseUrl/Account/Login" -SessionVariable session -UseBasicParsing
    $token = Get-AntiForgeryToken $login.Content
    Invoke-WebRequest -Uri "$baseUrl/Account/Login" -Method Post -WebSession $session -Body @{
        UserName = $UserName
        Password = 'Bonjour@2027'
        ReturnUrl = ''
        __RequestVerificationToken = $token
    } -UseBasicParsing | Out-Null
    return $session
}

function Invoke-FormPost(
    [Microsoft.PowerShell.Commands.WebRequestSession] $Session,
    [string] $Path,
    [hashtable] $Body,
    [string] $TokenSourcePath) {
    $tokenPage = Invoke-WebRequest -Uri "$baseUrl$TokenSourcePath" -WebSession $Session -UseBasicParsing
    $postBody = @{}
    foreach ($key in $Body.Keys) {
        $postBody[$key] = $Body[$key]
    }
    $postBody['__RequestVerificationToken'] = Get-AntiForgeryToken $tokenPage.Content
    return Invoke-WebRequest -Uri "$baseUrl$Path" -Method Post -WebSession $Session `
        -Body $postBody -UseBasicParsing -SkipHttpErrorCheck
}

function Get-StsHtml([string] $Date = $selectedDate, [string] $Shift = $selectedShift) {
    $encodedShift = [Uri]::EscapeDataString($Shift)
    return (Invoke-WebRequest -Uri "$baseUrl/Dispatch/Sts?date=$Date&shift=$encodedShift" `
        -WebSession $script:dispatcherSession -UseBasicParsing).Content
}

function Post-Sts([string] $Path, [hashtable] $Body) {
    $context = @{
        date = $selectedDate
        shift = $selectedShift
    }
    foreach ($key in $Body.Keys) {
        $context[$key] = $Body[$key]
    }
    return Invoke-FormPost $script:dispatcherSession $Path $context `
        "/Dispatch/Sts?date=$selectedDate&shift=$([Uri]::EscapeDataString($selectedShift))"
}

function Set-DispatchPost([string] $Poste) {
    Invoke-FormPost $script:dispatcherSession '/Account/ChooseDispatchPost' @{
        poste = $Poste
    } '/Account/ChooseDispatchPost' | Out-Null
}

function New-TestEscale([string] $Name, [string] $Voyage) {
    $create = Invoke-WebRequest -Uri "$baseUrl/Escales/Create" -WebSession $script:vpSession -UseBasicParsing
    $token = Get-AntiForgeryToken $create.Content
    Invoke-WebRequest -Uri "$baseUrl/Escales/Create" -Method Post -WebSession $script:vpSession -Body @{
        Navire = $Name
        Voyage = $Voyage
        LigneMaritime = 'RECETTE LINE'
        Eta = "${selectedDate}T06:00"
        VesselVisit = $Name
        Quai = 'Poste 1'
        Shift = $selectedShift
        Planificateur = 'Codex recette STS'
        ConfirmerDoublon = 'true'
        __RequestVerificationToken = $token
    } -UseBasicParsing | Out-Null

    $encoded = [Uri]::EscapeDataString($Name)
    $dashboard = (Invoke-WebRequest -Uri "$baseUrl/Escales?search=$encoded" `
        -WebSession $script:vpSession -UseBasicParsing).Content
    $idMatch = [regex]::Match($dashboard, '/Escales/Details/([0-9a-fA-F-]{36})')
    if (-not $idMatch.Success) {
        throw "Escale temporaire introuvable : $Name"
    }
    return $idMatch.Groups[1].Value
}

function Remove-TestEscale([string] $Id) {
    if ([string]::IsNullOrWhiteSpace($Id)) { return $false }
    $details = Invoke-WebRequest -Uri "$baseUrl/Escales/Details/$Id" -WebSession $script:vpSession `
        -UseBasicParsing -SkipHttpErrorCheck
    if ($details.StatusCode -eq 404) { return $true }
    $token = Get-AntiForgeryToken $details.Content
    Invoke-WebRequest -Uri "$baseUrl/Escales/Delete" -Method Post -WebSession $script:vpSession -Body @{
        id = $Id
        __RequestVerificationToken = $token
    } -UseBasicParsing -SkipHttpErrorCheck | Out-Null
    return (Invoke-WebRequest -Uri "$baseUrl/Escales/Details/$Id" -WebSession $script:vpSession `
        -UseBasicParsing -SkipHttpErrorCheck).StatusCode -eq 404
}

function Get-GantryId([string] $Html, [string] $Code) {
    $match = [regex]::Match(
        $Html,
        '>' + [regex]::Escape($Code) + '</span>.*?name="GantryId" value="([0-9a-fA-F-]{36})"',
        [Text.RegularExpressions.RegexOptions]::Singleline)
    return $(if ($match.Success) { $match.Groups[1].Value } else { $null })
}

function Get-GantryCard([string] $Html, [string] $Code) {
    $marker = ">$Code</span>"
    $markerIndex = $Html.IndexOf($marker, [StringComparison]::Ordinal)
    if ($markerIndex -lt 0) { return '' }
    $start = $Html.LastIndexOf('<div class="js-gantry-card', $markerIndex, [StringComparison]::Ordinal)
    $end = $Html.IndexOf('</form>', $markerIndex, [StringComparison]::Ordinal)
    if ($start -lt 0 -or $end -lt 0) { return '' }
    return $Html.Substring($start, ($end + 7) - $start)
}

function Get-RowContaining([string] $Html, [string] $Marker) {
    $index = $Html.IndexOf($Marker, [StringComparison]::Ordinal)
    if ($index -lt 0) { return '' }
    $start = $Html.LastIndexOf('<tr', $index, [StringComparison]::Ordinal)
    $end = $Html.IndexOf('</tr>', $index, [StringComparison]::Ordinal)
    if ($start -lt 0 -or $end -lt 0) { return '' }
    return $Html.Substring($start, ($end + 5) - $start)
}

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
    $start = $Html.IndexOf('<h2 class="card-title mb-5">Affectations</h2>', [StringComparison]::Ordinal)
    $end = $Html.IndexOf('<form action="/Dispatch/AssignGantry"', $start, [StringComparison]::Ordinal)
    if ($start -lt 0 -or $end -lt 0) { return '' }
    return $Html.Substring($start, $end - $start)
}

function Get-GuidAfter([string] $Html, [string] $Marker, [string] $InputName) {
    $pattern = [regex]::Escape($Marker) +
        '.*?name="' + [regex]::Escape($InputName) +
        '" value="([0-9a-fA-F-]{36})"'
    $match = [regex]::Match($Html, $pattern, [Text.RegularExpressions.RegexOptions]::Singleline)
    return $(if ($match.Success) { $match.Groups[1].Value } else { $null })
}

function Get-SelectOptionsAfter([string] $Html, [string] $Heading, [string] $SelectName) {
    $headingIndex = $Html.IndexOf($Heading, [StringComparison]::Ordinal)
    if ($headingIndex -lt 0) { return @() }
    $tail = $Html.Substring($headingIndex)
    $select = [regex]::Match(
        $tail,
        '<select[^>]*name="' + [regex]::Escape($SelectName) + '"[^>]*>(.*?)</select>',
        [Text.RegularExpressions.RegexOptions]::Singleline)
    if (-not $select.Success) { return @() }
    return @([regex]::Matches(
        $select.Groups[1].Value,
        '<option[^>]*>(.*?)</option>',
        [Text.RegularExpressions.RegexOptions]::Singleline) | ForEach-Object {
            [Net.WebUtility]::HtmlDecode(
                [regex]::Replace($_.Groups[1].Value, '<[^>]+>', '')).Trim()
        })
}

New-Item -ItemType Directory -Path (Split-Path $evidencePath) -Force | Out-Null

try {
    $vpSession = New-AuthenticatedSession 'vplanner'
    $dispatcherSession = New-AuthenticatedSession 'dispatcher1'
    Set-DispatchPost 'STS'

    $escaleIdA = New-TestEscale $escaleNameA 'STS4-A'
    $escaleIdB = New-TestEscale $escaleNameB 'STS4-B'
    Write-Output "ESCALES_TEST=$escaleIdA,$escaleIdB"

    $html = Get-StsHtml

    # STS-01 : date et shift visibles et modifiables.
    $dateInput = [regex]::Match($html, '<input type="date" name="date"[^>]*>').Value
    $shiftSelect = [regex]::Match(
        $html,
        '<select name="shift"[^>]*>(.*?)</select>',
        [Text.RegularExpressions.RegexOptions]::Singleline).Value
    Record-Evidence 'STS-01' (
        $dateInput.Contains("value=`"$selectedDate`"") -and
        -not $dateInput.Contains('disabled') -and
        -not $dateInput.Contains('readonly') -and
        $shiftSelect -match 'value="Matin" selected') 'Date 24/07/2026 et shift Matin affichés dans des contrôles modifiables.'

    # STS-03 : le tableau navires ne contient aucun contrôle de modification.
    $vesselRow = Get-RowContaining $html $escaleNameA
    Record-Evidence 'STS-03' (
        $vesselRow.Contains($escaleNameA) -and
        -not $vesselRow.Contains('<input') -and
        -not $vesselRow.Contains('<select')) 'La ligne navire est rendue en lecture seule.'

    # STS-04 : pool fixe des huit portiques.
    $gantryCodes = 1..8 | ForEach-Object { "CR$_" }
    $missingGantries = @($gantryCodes | Where-Object { -not $html.Contains(">$_</span>") })
    Record-Evidence 'STS-04' ($missingGantries.Count -eq 0) 'CR1 à CR8 sont tous présents.'

    $cr1 = Get-GantryId $html 'CR1'
    $cr2 = Get-GantryId $html 'CR2'
    $cr3 = Get-GantryId $html 'CR3'

    # STS-05/06 : passage en panne, apparence et saisie inline cause/type.
    Post-Sts '/Dispatch/ChangeGantryStatus' @{
        GantryId = $cr1
        Statut = 2
        EscaleId = $escaleIdA
        TypeIncident = 'Panne mécanique'
        Cause = 'RECETTE PANNE INLINE'
        DateDebutUtc = "${selectedDate}T05:00"
    } | Out-Null
    $html = Get-StsHtml
    $cr1Card = Get-GantryCard $html 'CR1'
    Record-Evidence 'STS-05' (
        $cr1Card.Contains('badge-danger') -and $cr1Card.Contains('En panne')) 'CR1 passe En panne et sa bulle prend l''apparence danger.'
    Record-Evidence 'STS-06' (
        $cr1Card.Contains('js-gantry-breakdown-fields space-y-2 "') -and
        -not $cr1Card.Contains('js-gantry-breakdown-fields space-y-2 hidden') -and
        $cr1Card.Contains('name="TypeIncident"') -and
        $cr1Card.Contains('name="Cause"') -and
        -not $cr1Card.Contains('modal')) 'Type de panne et cause apparaissent directement dans la bulle, sans fenêtre.'

    # STS-07 : un portique en panne ne peut pas être affecté.
    Post-Sts '/Dispatch/AssignGantry' @{
        GantryId = $cr1
        EscaleId = $escaleIdA
        HeureDebut = "${selectedDate}T06:00"
        TacheOuZone = 'REFUS-PANNE-STS4'
    } | Out-Null
    $html = Get-StsHtml
    $assignmentArea = Get-AssignmentArea $html
    Record-Evidence 'STS-07' (-not $assignmentArea.Contains('REFUS-PANNE-STS4')) 'Aucune affectation n''est créée pour CR1 en panne.'

    # STS-08 : remise en service et clôture automatique de l''incident.
    Post-Sts '/Dispatch/ChangeGantryStatus' @{
        GantryId = $cr1
        Statut = 0
        EscaleId = ''
        TypeIncident = ''
        Cause = ''
        DateDebutUtc = ''
    } | Out-Null
    $html = Get-StsHtml
    $incidentRow = Get-IncidentEntryBlock $html 'RECETTE PANNE INLINE'
    Record-Evidence 'STS-08' (
        $incidentRow.Contains('Repris') -and
        $incidentRow -match '\d{2}/\d{2} \d{2}:\d{2}') 'La remise en service clôture automatiquement la panne avec une heure de fin.'

    # STS-09/10 : affectation via le contrat du formulaire de drag/drop, début automatique.
    $dragContractPresent = $html.Contains('id="stsDragAssignForm"') -and
        $html.Contains('draggable="true"') -and
        $html.Contains('js-vessel-dropzone') -and
        $html.Contains("form.submit();")
    $beforeAssign = [DateTime]::UtcNow
    Post-Sts '/Dispatch/AssignGantry' @{
        GantryId = $cr2
        EscaleId = $escaleIdA
        TacheOuZone = 'DRAGDROP-STS4'
    } | Out-Null
    $afterAssign = [DateTime]::UtcNow
    $html = Get-StsHtml
    $assignmentRow = Get-RowContaining $html 'DRAGDROP-STS4'
    $assignmentId = Get-GuidAfter $assignmentRow 'DRAGDROP-STS4' 'assignmentId'
    if (-not $assignmentId) {
        $assignmentId = Get-GuidAfter $assignmentRow 'CR2' 'AssignmentId'
    }
    Record-Evidence 'STS-09' (
        $dragContractPresent -and
        $assignmentRow.Contains('CR2') -and
        $assignmentRow.Contains($escaleNameA)) 'Le drag/drop est câblé vers AssignGantry et l''affectation CR2/navire est créée.'

    $startMatch = [regex]::Match($assignmentRow, 'name="HeureDebut" value="([^"]+)"')
    $autoStart = [DateTime]::MinValue
    $autoStartParsed = $startMatch.Success -and [DateTime]::TryParse($startMatch.Groups[1].Value, [ref]$autoStart)
    $selectedOperationalDate = [DateTime]::ParseExact($selectedDate, 'yyyy-MM-dd', [Globalization.CultureInfo]::InvariantCulture).Date
    Record-Evidence 'STS-10' (
        $autoStartParsed -and
        $autoStart.Date -eq $selectedOperationalDate -and
        $autoStart.TimeOfDay -ge $beforeAssign.AddMinutes(-1).TimeOfDay -and
        $autoStart.TimeOfDay -le $afterAssign.AddMinutes(1).TimeOfDay) ("Début automatique enregistré sur la date opérationnelle : " + $(if ($autoStartParsed) { $autoStart.ToString('o') } else { 'introuvable' }))

    # STS-11 : correction persistée de l''heure de début.
    if (-not $assignmentId) {
        $assignmentId = ([regex]::Match($assignmentRow, 'name="AssignmentId" value="([0-9a-fA-F-]{36})"')).Groups[1].Value
    }
    Post-Sts '/Dispatch/UpdateAssignment' @{
        AssignmentId = $assignmentId
        HeureDebut = "${selectedDate}T08:00"
        HeureFin = ''
    } | Out-Null
    $html = Get-StsHtml
    $assignmentRow = Get-RowContaining $html 'DRAGDROP-STS4'
    Record-Evidence 'STS-11' (
        $assignmentRow.Contains('value="2026-07-24T08:00"')) 'L''heure corrigée à 08:00 est restituée après rechargement.'

    # STS-12/13 : retrait, conservation et durée exacte.
    Post-Sts '/Dispatch/EndAssignment' @{
        assignmentId = $assignmentId
        heureFin = "${selectedDate}T09:00"
    } | Out-Null
    $html = Get-StsHtml
    $assignmentRow = Get-RowContaining $html 'DRAGDROP-STS4'
    Record-Evidence 'STS-12' (
        $assignmentRow.Contains('Terminée') -and
        $assignmentRow.Contains('24/07 09:00')) 'Le retrait clôture la ligne à 09:00 sans la supprimer.'
    Record-Evidence 'STS-13' ($assignmentRow.Contains('1h00')) 'Durée calculée automatiquement : 1h00.'

    # STS-14 : nouvelle affectation après retrait sans écraser l''historique.
    Post-Sts '/Dispatch/AssignGantry' @{
        GantryId = $cr2
        EscaleId = $escaleIdB
        HeureDebut = "${selectedDate}T10:00"
        TacheOuZone = 'REAFFECTATION-STS4'
    } | Out-Null
    $html = Get-StsHtml
    $row2 = Get-RowContaining $html 'REAFFECTATION-STS4'
    $assignmentId2 = ([regex]::Match($row2, 'name="AssignmentId" value="([0-9a-fA-F-]{36})"')).Groups[1].Value
    Record-Evidence 'STS-14' (
        $html.Contains('DRAGDROP-STS4') -and
        $row2.Contains($escaleNameB) -and
        $assignmentId2 -ne $assignmentId) 'La seconde affectation est créée sur l''autre navire et la première reste visible.'
    Post-Sts '/Dispatch/EndAssignment' @{
        assignmentId = $assignmentId2
        heureFin = "${selectedDate}T10:30"
    } | Out-Null

    # STS-15 : incident portique complet avec bornes.
    Post-Sts '/Dispatch/AddStsIncident' @{
        EscaleId = $escaleIdA
        GantryId = $cr3
        TypeIncident = 'Panne automate'
        DateDebutUtc = "${selectedDate}T10:00"
        DateFinUtc = "${selectedDate}T11:00"
        Cause = 'RECETTE INCIDENT STS4'
        RetirePortiqueEffectif = 'true'
    } | Out-Null
    $html = Get-StsHtml
    $incidentRow = Get-IncidentEntryBlock $html 'RECETTE INCIDENT STS4'
    $incidentId = ([regex]::Match($incidentRow, 'name="IncidentId" value="([0-9a-fA-F-]{36})"')).Groups[1].Value
    Record-Evidence 'STS-15' (
        $incidentRow.Contains($escaleNameA) -and
        $incidentRow.Contains('CR3') -and
        $incidentRow.Contains('24/07 10:00') -and
        $incidentRow.Contains('24/07 11:00')) 'Incident CR3 affiché avec navire, type, début et fin.'

    # STS-16 : modification navire/type/heures et recalcul.
    Post-Sts '/Dispatch/UpdateStsIncident' @{
        IncidentId = $incidentId
        EscaleId = $escaleIdB
        GantryId = $cr3
        TypeIncident = 'Panne électrique'
        DateDebutUtc = "${selectedDate}T11:00"
        DateFinUtc = "${selectedDate}T13:00"
        Cause = 'RECETTE INCIDENT MODIFIE'
        ConditionsReprise = 'Test terminé'
        RetirePortiqueEffectif = 'true'
    } | Out-Null
    $html = Get-StsHtml
    $incidentRow = Get-IncidentEntryBlock $html 'RECETTE INCIDENT MODIFIE'
    Record-Evidence 'STS-16' (
        $incidentRow.Contains($escaleNameB) -and
        $incidentRow.Contains('24/07 11:00') -and
        $incidentRow.Contains('24/07 13:00') -and
        $incidentRow.Contains('2h00')) 'Navire, type et heures modifiés ; durée recalculée à 2h00.'

    # STS-17 : registres et listes distincts.
    Post-Sts '/Dispatch/AddStsIncident' @{
        EscaleId = $escaleIdA
        GantryId = ''
        TypeIncident = 'Attente documents navire'
        DateDebutUtc = "${selectedDate}T14:00"
        DateFinUtc = "${selectedDate}T14:30"
        Cause = 'RECETTE INCIDENT NAVIRE'
        RetirePortiqueEffectif = 'false'
    } | Out-Null
    $html = Get-StsHtml
    $vesselTypes = Get-SelectOptionsAfter $html '>Incident navire</h3>' 'TypeIncident'
    $gantryTypes = Get-SelectOptionsAfter $html '>Panne portique</h3>' 'TypeIncident'
    Record-Evidence 'STS-17' (
        $html.Contains('>Incident navire</h3>') -and
        $html.Contains('>Panne portique</h3>') -and
        $vesselTypes.Contains('Attente documents navire') -and
        -not $vesselTypes.Contains('Panne spreader') -and
        $gantryTypes.Contains('Panne spreader') -and
        -not $gantryTypes.Contains('Attente documents navire')) 'Deux registres visibles avec deux listes de causes réellement distinctes.'

    # STS-18 : pointeur Terre avec horaires et durée.
    Post-Sts '/Dispatch/AddStsPointeur' @{
        Nom = $pointerName
        Role = 'Terre'
        NavireOuZone = $escaleNameA
        HeurePriseDePosteUtc = "${selectedDate}T07:00"
        HeureFinUtc = "${selectedDate}T08:30"
    } | Out-Null
    $html = Get-StsHtml
    $pointerRow = Get-RowContaining $html $pointerName
    Record-Evidence 'STS-18' (
        $pointerRow.Contains('Terre') -and
        $pointerRow.Contains('1h30')) 'Pointeur Terre ajouté avec durée calculée de 1h30.'

    # STS-19 : ROPN avec horaires, difficulté et durée.
    Post-Sts '/Dispatch/AddRopnEntry' @{
        Nom = $ropnName
        Role = 'Bord'
        DateDebutUtc = "${selectedDate}T06:00"
        DateFinUtc = "${selectedDate}T07:15"
        DifficulteRencontree = 'RECETTE DIFFICULTE ROPN'
    } | Out-Null
    $html = Get-StsHtml
    $ropnRow = Get-RowContaining $html $ropnName
    Record-Evidence 'STS-19' (
        $ropnRow.Contains('RECETTE DIFFICULTE ROPN') -and
        $ropnRow.Contains('1h15')) 'ROPN ajouté avec bornes horaires et durée calculée de 1h15.'

    # STS-02 : séparation par date, puis retour sans perte.
    $otherDateHtml = Get-StsHtml '2026-07-25' $selectedShift
    $backHtml = Get-StsHtml
    Record-Evidence 'STS-02' (
        -not $otherDateHtml.Contains($pointerName) -and
        -not $otherDateHtml.Contains($ropnName) -and
        $backHtml.Contains($pointerName) -and
        $backHtml.Contains($ropnName)) 'Les saisies du 24/07 sont absentes le 25/07 puis réapparaissent au retour.'

    # STS-20 : répartition TT transmise au même navire.
    Set-DispatchPost 'TT'
    Invoke-FormPost $dispatcherSession '/Dispatch/AssignTt' @{
        EscaleId = $escaleIdA
        NombrePrevu = 8
        NombreAffecte = 7
        NombreOperationnel = 7
        Observations = 'RECETTE TRANSMISSION STS4'
    } '/Dispatch/Tt' | Out-Null
    Set-DispatchPost 'STS'
    $html = Get-StsHtml
    $vesselRow = Get-RowContaining $html $escaleNameA
    Record-Evidence 'STS-20' (
        $vesselRow.Contains('7 TT')) 'La répartition TT de 7 tracteurs est reprise sur la ligne du navire côté STS.'
}
catch {
    $allPassed = $false
    $evidence.Add([pscustomobject]@{
        scenario = 'ERREUR-HARNAIS'
        passed = $false
        actual = $_.Exception.Message
        proof = $_.ScriptStackTrace
        testedAtUtc = [DateTime]::UtcNow.ToString('o')
    })
    Write-Output ('ERREUR-HARNAIS: ' + $_.Exception.Message)
    try {
        [IO.File]::WriteAllText($failureHtmlPath, (Get-StsHtml), [Text.UTF8Encoding]::new($false))
    } catch {
        Write-Output 'Impossible de sauvegarder le HTML d''échec.'
    }
}
finally {
    try {
        if ($dispatcherSession) {
            Set-DispatchPost 'STS'
        }
    } catch {
        Write-Output 'Restauration du poste STS impossible.'
        $allPassed = $false
    }

    $deletedA = $false
    $deletedB = $false
    try { $deletedA = Remove-TestEscale $escaleIdA } catch { Write-Output $_.Exception.Message }
    try { $deletedB = Remove-TestEscale $escaleIdB } catch { Write-Output $_.Exception.Message }

    $sqlEscapedPointer = $pointerName.Replace("'", "''")
    $sqlEscapedRopn = $ropnName.Replace("'", "''")
    & sqlcmd -S '(localdb)\MSSQLLocalDB' -d EscaleReport -E -b -Q `
        "DELETE FROM StsPointeurs WHERE Nom = N'$sqlEscapedPointer'; DELETE FROM RopnEntries WHERE Nom = N'$sqlEscapedRopn';" | Out-Null
    $sqlCleanup = $LASTEXITCODE -eq 0

    $cleanupPassed = $deletedA -and $deletedB -and $sqlCleanup
    Record-Evidence 'NETTOYAGE-WAVE4-STS' $cleanupPassed `
        "Escales A/B supprimées=$deletedA/$deletedB ; Pointeur/ROPN temporaires supprimés=$sqlCleanup." `
        'HTTP cascade + SQL local ciblé par noms uniques'

    $evidenceDocument = [ordered]@{
        wave = 4
        module = 'Dispatch STS'
        testedAtUtc = [DateTime]::UtcNow.ToString('o')
        baseUrl = $baseUrl
        build = 'C:\tmp\escalereport-wave4-build4\EscaleReport.Web.dll'
        testEscales = @($escaleNameA, $escaleNameB)
        allPassed = $allPassed
        results = $evidence
    }
    [IO.File]::WriteAllText(
        $evidencePath,
        ($evidenceDocument | ConvertTo-Json -Depth 7),
        [Text.UTF8Encoding]::new($false))
}

Write-Output "EVIDENCE=$evidencePath"
Write-Output "ALL_PASSED=$allPassed"
