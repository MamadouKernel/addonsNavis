$ErrorActionPreference = 'Stop'

function Replace-Normalized {
    param(
        [Parameter(Mandatory)] [string] $Path,
        [Parameter(Mandatory)] [string] $Old,
        [Parameter(Mandatory)] [string] $New
    )
    $resolved = (Resolve-Path -LiteralPath $Path).Path
    $content = [IO.File]::ReadAllText($resolved).Replace("`r`n", "`n")
    $oldNormalized = $Old.Replace("`r`n", "`n")
    $newNormalized = $New.Replace("`r`n", "`n")
    if (-not $content.Contains($oldNormalized)) {
        throw "Bloc introuvable dans $Path"
    }
    [IO.File]::WriteAllText(
        $resolved,
        $content.Replace($oldNormalized, $newNormalized),
        [Text.UTF8Encoding]::new($false))
}

$handler = '.\EscaleReport.Web\Application\Dispatch\Queries\GetDispatchSts\GetDispatchStsQueryHandler.cs'

Replace-Normalized $handler `
@'
        if (!DispatchAccessControl.CanAccessPoste(currentUser, "STS"))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }


        var gantries
'@ `
@'
        if (!DispatchAccessControl.CanAccessPoste(currentUser, "STS"))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var selectedDate = request.Date ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var dayStart = selectedDate.ToDateTime(TimeOnly.MinValue);
        var dayEnd = dayStart.AddDays(1);

        var gantries
'@

Replace-Normalized $handler `
@'
             join g in dbContext.Gantries.AsNoTracking() on a.GantryId equals g.Id
             join e in dbContext.Escales.AsNoTracking() on a.EscaleId equals e.Id
             orderby a.Statut, a.HeureDebut descending
'@ `
@'
             join g in dbContext.Gantries.AsNoTracking() on a.GantryId equals g.Id
             join e in dbContext.Escales.AsNoTracking() on a.EscaleId equals e.Id
             where a.HeureDebut >= dayStart && a.HeureDebut < dayEnd
                && (string.IsNullOrEmpty(request.Shift) || string.IsNullOrEmpty(e.Shift) || e.Shift == request.Shift)
             orderby a.Statut, a.HeureDebut descending
'@

Replace-Normalized $handler `
@'
                 HeureFin = a.HeureFin,
                 TacheOuZone = a.TacheOuZone,
'@ `
@'
                 HeureFin = a.HeureFin,
                 Duree = a.HeureFin.HasValue ? a.HeureFin.Value - a.HeureDebut : null,
                 TacheOuZone = a.TacheOuZone,
'@

Replace-Normalized $handler `
@'
             from i in dbContext.StsIncidents.AsNoTracking()
             join e in dbContext.Escales.AsNoTracking() on i.EscaleId equals e.Id
             orderby i.DateDebutUtc descending
'@ `
@'
             from i in dbContext.StsIncidents.AsNoTracking()
             join e in dbContext.Escales.AsNoTracking() on i.EscaleId equals e.Id
             where i.DateDebutUtc >= dayStart && i.DateDebutUtc < dayEnd
                && (string.IsNullOrEmpty(request.Shift) || string.IsNullOrEmpty(e.Shift) || e.Shift == request.Shift)
             orderby i.DateDebutUtc descending
'@

Replace-Normalized $handler `
@'
             Id = x.i.Id,
             Navire = x.Navire,
'@ `
@'
             Id = x.i.Id,
             EscaleId = x.i.EscaleId,
             GantryId = x.i.GantryId,
             Navire = x.Navire,
'@

Replace-Normalized $handler `
@'
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
'@ `
@'
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

Replace-Normalized $handler `
@'
        // CDC §6.1 "Sélection du shift" : affichage automatique des navires en cours
        // d'opération ou attendus (ETA) pendant la date/shift sélectionnés.
        var selectedDate = request.Date ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var shiftsDisponibles
'@ `
@'
        // CDC §6.1 "Sélection du shift" : affichage automatique des navires en cours
        // d'opération ou attendus (ETA) pendant la date/shift sélectionnés.
        var shiftsDisponibles
'@

Replace-Normalized $handler `
@'
                 Shift = e.Shift,
                 EnCours = e.StatutOperations == StatutOperations.EnCours
'@ `
@'
                 Shift = e.Shift,
                 EnCours = e.StatutOperations == StatutOperations.EnCours,
                 NombreTracteurs = tracteursParEscale.GetValueOrDefault(e.Id)
'@

Replace-Normalized $handler `
@'
             Incidents = PagedResult<StsIncidentDto>.Create(incidents, request.IncidentsPage),
             TypesIncidentDisponibles = typesIncident,
             Pointeurs
'@ `
@'
             Incidents = PagedResult<StsIncidentDto>.Create(incidents, request.IncidentsPage),
             TypesPannePortiqueDisponibles = typesPannePortique,
             TypesIncidentNavireDisponibles = typesIncidentNavire,
             Pointeurs
'@

$dto = '.\EscaleReport.Web\Application\Dispatch\Queries\GetDispatchSts\DispatchStsDto.cs'

Replace-Normalized $dto `
@'
    public PagedResult<StsIncidentDto> Incidents { get; set; } = new();
    public IReadOnlyList<string> TypesIncidentDisponibles { get; set; } = [];
'@ `
@'
    public PagedResult<StsIncidentDto> Incidents { get; set; } = new();
    public IReadOnlyList<string> TypesPannePortiqueDisponibles { get; set; } = [];
    public IReadOnlyList<string> TypesIncidentNavireDisponibles { get; set; } = [];
'@

Replace-Normalized $dto `
@'
    public string? Shift { get; set; }
    public bool EnCours { get; set; }
'@ `
@'
    public string? Shift { get; set; }
    public bool EnCours { get; set; }
    public int NombreTracteurs { get; set; }
'@

Write-Output 'Query et DTO du tableau STS vague 4 mis à jour.'
