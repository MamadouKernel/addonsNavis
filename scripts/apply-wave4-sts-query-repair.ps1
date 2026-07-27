$ErrorActionPreference = 'Stop'

function Load-Normalized([string] $Path) {
    return [IO.File]::ReadAllText((Resolve-Path -LiteralPath $Path).Path).Replace("`r`n", "`n")
}

function Save-Normalized([string] $Path, [string] $Content) {
    [IO.File]::WriteAllText(
        (Resolve-Path -LiteralPath $Path).Path,
        $Content,
        [Text.UTF8Encoding]::new($false))
}

function Replace-Required([string] $Content, [string] $Old, [string] $New, [string] $Label) {
    if (-not $Content.Contains($Old)) {
        throw "Bloc introuvable : $Label"
    }
    return $Content.Replace($Old, $New)
}

$handler = '.\EscaleReport.Web\Application\Dispatch\Queries\GetDispatchSts\GetDispatchStsQueryHandler.cs'
$content = Load-Normalized $handler

$content = Replace-Required $content `
    '            join e in dbContext.Escales.AsNoTracking() on a.EscaleId equals e.Id
            orderby a.Statut, a.HeureDebut descending' `
    '            join e in dbContext.Escales.AsNoTracking() on a.EscaleId equals e.Id
            where a.HeureDebut >= dayStart && a.HeureDebut < dayEnd
                && (string.IsNullOrEmpty(request.Shift) || string.IsNullOrEmpty(e.Shift) || e.Shift == request.Shift)
            orderby a.Statut, a.HeureDebut descending' `
    'filtre affectations'

$content = Replace-Required $content `
    '                HeureFin = a.HeureFin,
                TacheOuZone = a.TacheOuZone,' `
    '                HeureFin = a.HeureFin,
                Duree = a.HeureFin.HasValue ? a.HeureFin.Value - a.HeureDebut : null,
                TacheOuZone = a.TacheOuZone,' `
    'durée affectation'

$content = Replace-Required $content `
    '            join e in dbContext.Escales.AsNoTracking() on i.EscaleId equals e.Id
            orderby i.DateDebutUtc descending' `
    '            join e in dbContext.Escales.AsNoTracking() on i.EscaleId equals e.Id
            where i.DateDebutUtc >= dayStart && i.DateDebutUtc < dayEnd
                && (string.IsNullOrEmpty(request.Shift) || string.IsNullOrEmpty(e.Shift) || e.Shift == request.Shift)
            orderby i.DateDebutUtc descending' `
    'filtre incidents'

$content = Replace-Required $content `
    '            Id = x.i.Id,
            Navire = x.Navire,' `
    '            Id = x.i.Id,
            EscaleId = x.i.EscaleId,
            GantryId = x.i.GantryId,
            Navire = x.Navire,' `
    'identifiants incident'

$oldLists = @'
        var typesIncident = await dbContext.ReferenceValues
            .AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.StsIncidentType && r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);

        var pointeurs = await dbContext.StsPointeurs
            .AsNoTracking()
            .OrderByDescending(p => p.HeurePriseDePosteUtc)
            .ToListAsync(cancellationToken);

        var ropn = await dbContext.RopnEntries
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAtUtc)
            .ToListAsync(cancellationToken);
'@
$newLists = @'
        var typesPannePortique = await dbContext.ReferenceValues
            .AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.StsIncidentType && r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);

        var typesIncidentNavire = await dbContext.ReferenceValues
            .AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.StsVesselIncidentType && r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);

        var pointeurs = await dbContext.StsPointeurs
            .AsNoTracking()
            .Where(p => p.HeurePriseDePosteUtc >= dayStart && p.HeurePriseDePosteUtc < dayEnd)
            .OrderByDescending(p => p.HeurePriseDePosteUtc)
            .ToListAsync(cancellationToken);

        var ropn = await dbContext.RopnEntries
            .AsNoTracking()
            .Where(r => r.DateDebutUtc >= dayStart && r.DateDebutUtc < dayEnd)
            .OrderByDescending(r => r.DateDebutUtc)
            .ToListAsync(cancellationToken);

        var ttAssignments = await dbContext.TtVesselAssignments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var tracteursParEscale = ttAssignments
            .GroupBy(a => a.EscaleId)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(a => a.CreatedAtUtc).First().NombreAffecte);
'@
$content = Replace-Required $content $oldLists $newLists 'listes et reprise TT'

$content = Replace-Required $content `
    '        var selectedDate = request.Date ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var shiftsDisponibles' `
    '        var shiftsDisponibles' `
    'suppression selectedDate dupliquée'

$content = Replace-Required $content `
    '                Shift = e.Shift,
                EnCours = e.StatutOperations == StatutOperations.EnCours' `
    '                Shift = e.Shift,
                EnCours = e.StatutOperations == StatutOperations.EnCours,
                NombreTracteurs = tracteursParEscale.GetValueOrDefault(e.Id)' `
    'tracteurs dans navire'

$content = Replace-Required $content `
    '            TypesIncidentDisponibles = typesIncident,' `
    '            TypesPannePortiqueDisponibles = typesPannePortique,
            TypesIncidentNavireDisponibles = typesIncidentNavire,' `
    'types dans DTO'

Save-Normalized $handler $content

$dto = '.\EscaleReport.Web\Application\Dispatch\Queries\GetDispatchSts\DispatchStsDto.cs'
$dtoContent = Load-Normalized $dto
$dtoContent = Replace-Required $dtoContent `
    '    public IReadOnlyList<string> TypesIncidentDisponibles { get; set; } = [];' `
    '    public IReadOnlyList<string> TypesPannePortiqueDisponibles { get; set; } = [];
    public IReadOnlyList<string> TypesIncidentNavireDisponibles { get; set; } = [];' `
    'propriétés types DTO'
$dtoContent = Replace-Required $dtoContent `
    '    public bool EnCours { get; set; }' `
    '    public bool EnCours { get; set; }
    public int NombreTracteurs { get; set; }' `
    'propriété tracteurs DTO'
Save-Normalized $dto $dtoContent

Write-Output 'Réparation query STS appliquée.'
