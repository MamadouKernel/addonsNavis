$ErrorActionPreference = 'Stop'

function Update-ExactFile {
    param(
        [Parameter(Mandatory)] [string] $Path,
        [Parameter(Mandatory)] [hashtable[]] $Replacements
    )

    $resolved = (Resolve-Path -LiteralPath $Path).Path
    $text = [IO.File]::ReadAllText($resolved).Replace("`r`n", "`n")
    foreach ($replacement in $Replacements) {
        $old = ([string]$replacement.Old).Replace("`r`n", "`n")
        $new = ([string]$replacement.New).Replace("`r`n", "`n")
        if (-not $text.Contains($old)) {
            throw "Bloc attendu introuvable dans $Path : $($old.Substring(0, [Math]::Min(80, $old.Length)))"
        }
        $text = $text.Replace($old, $new)
    }
    [IO.File]::WriteAllText($resolved, $text.Replace("`n", [Environment]::NewLine), [Text.UTF8Encoding]::new($false))
}

Update-ExactFile -Path 'EscaleReport.Web\Domain\Common\ReferenceValue.cs' -Replacements @(
    @{
        Old = @'
    public const string IncidentCategory = "IncidentCategory";
    public const string CutReason = "CutReason";
'@
        New = @'
    public const string IncidentCategory = "IncidentCategory";
    public const string IncidentSeverity = "IncidentSeverity";
    public const string CutReason = "CutReason";
'@
    },
    @{
        Old = @'
        [IncidentCategory] = "Catégories d'incident",
        [CutReason] = "Motifs de coupure",
'@
        New = @'
        [IncidentCategory] = "Catégories d'incident",
        [IncidentSeverity] = "Gravités d'incident",
        [CutReason] = "Motifs de coupure",
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Infrastructure\Persistence\DbSeeder.cs' -Replacements @(
    @{
        Old = @'
        await SeedIncidentCategoriesAsync(dbContext);
        await SeedStsIncidentTypesAsync(dbContext);
'@
        New = @'
        await SeedIncidentCategoriesAsync(dbContext);
        await SeedIncidentSeveritiesAsync(dbContext);
        await SeedStsIncidentTypesAsync(dbContext);
'@
    },
    @{
        Old = @'
        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedStsIncidentTypesAsync
'@
        New = @'
        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedIncidentSeveritiesAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.ReferenceValues.AnyAsync(r => r.ListKey == ReferenceListKeys.IncidentSeverity))
        {
            return;
        }

        string[] severities = ["Information", "Moyen", "Critique"];
        for (var i = 0; i < severities.Length; i++)
        {
            dbContext.ReferenceValues.Add(new ReferenceValue
            {
                ListKey = ReferenceListKeys.IncidentSeverity,
                Value = severities[i],
                SortOrder = i
            });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedStsIncidentTypesAsync
'@
    },
    @{
        Old = @'
                Permissions.CreerEscale,
                Permissions.ModifierEscale,
                Permissions.SupprimerEscale,
                Permissions.MarquerEscaleTerminee,
                Permissions.SaisirDonneesModule,
                Permissions.ModifierDonneesModule
            ]);
'@
        New = @'
                Permissions.CreerEscale,
                Permissions.ModifierEscale,
                Permissions.SupprimerEscale,
                Permissions.MarquerEscaleTerminee,
                Permissions.SaisirDonneesModule,
                Permissions.ModifierDonneesModule,
                Permissions.GenererPdf,
                Permissions.GenererExcel
            ]);
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Application\Escales\Queries\GetEscaleDetail\EscaleDetailDto.cs' -Replacements @(
    @{
        Old = @'
using EscaleReport.Web.Application.VesselPlanning.Dtos;
'@
        New = @'
using EscaleReport.Web.Application.VesselPlanning.Dtos;
using EscaleReport.Web.Domain.VesselPlanning;
'@
    },
    @{
        Old = @'
    public IReadOnlyList<string> CategoriesIncidentDisponibles { get; set; } = [];
    public int IncidentsEnCoursCount { get; set; }
'@
        New = @'
    public IReadOnlyList<string> CategoriesIncidentDisponibles { get; set; } = [];
    public IReadOnlyList<IncidentGravite> GravitesIncidentDisponibles { get; set; } = [];
    public int IncidentsEnCoursCount { get; set; }
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Application\Escales\Queries\GetEscaleDetail\GetEscaleDetailQueryHandler.cs' -Replacements @(
    @{
        Old = @'
using EscaleReport.Web.Domain.Identity;
using MediatR;
'@
        New = @'
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;
'@
    },
    @{
        Old = @'
        var additionnels = await dbContext.AdditionalContainers
'@
        New = @'
        var graviteValues = await dbContext.ReferenceValues
            .AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.IncidentSeverity && r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);
        var gravitesIncident = graviteValues
            .Select(value => Enum.TryParse<IncidentGravite>(value, true, out var severity)
                ? (IncidentGravite?)severity
                : null)
            .Where(severity => severity.HasValue)
            .Select(severity => severity!.Value)
            .ToList();

        var additionnels = await dbContext.AdditionalContainers
'@
    },
    @{
        Old = @'
            CategoriesIncidentDisponibles = categoriesIncident,
            IncidentsEnCoursCount = incidentsDto.Count(i => i.Statut == Domain.VesselPlanning.IncidentStatus.EnCours),
'@
        New = @'
            CategoriesIncidentDisponibles = categoriesIncident,
            GravitesIncidentDisponibles = gravitesIncident,
            IncidentsEnCoursCount = incidentsDto.Count(i => i.Statut == IncidentStatus.EnCours),
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Application\Escales\Dtos\EscaleDto.cs' -Replacements @(
    @{
        Old = @'
    public string? UpdatedBy { get; set; }
'@
        New = @'
    public string? UpdatedBy { get; set; }
    public int AnomaliesNonResoluesCount { get; set; }
    public int AdditionnelsCount { get; set; }
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Application\Escales\Queries\GetEscales\GetEscalesQueryHandler.cs' -Replacements @(
    @{
        Old = @'
        var escales = await orderedQuery.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

        return new PagedResult<EscaleDto>
        {
            Items = escales.Select(EscaleDto.FromEntity).ToList(),
'@
        New = @'
        var escales = await orderedQuery.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);
        var escaleIds = escales.Select(e => e.Id).ToList();

        var anomalyCounts = await dbContext.ContainerAnomalies
            .AsNoTracking()
            .Where(a => escaleIds.Contains(a.EscaleId) && a.Statut == Domain.VesselPlanning.AnomalyStatus.NonResolu)
            .GroupBy(a => a.EscaleId)
            .ToDictionaryAsync(group => group.Key, group => group.Count(), cancellationToken);

        var additionalCounts = await dbContext.AdditionalContainers
            .AsNoTracking()
            .Where(c => escaleIds.Contains(c.EscaleId))
            .GroupBy(c => c.EscaleId)
            .ToDictionaryAsync(group => group.Key, group => group.Count(), cancellationToken);

        return new PagedResult<EscaleDto>
        {
            Items = escales.Select(escale =>
            {
                var dto = EscaleDto.FromEntity(escale);
                dto.AnomaliesNonResoluesCount = anomalyCounts.GetValueOrDefault(escale.Id);
                dto.AdditionnelsCount = additionalCounts.GetValueOrDefault(escale.Id);
                return dto;
            }).ToList(),
'@
    }
)

Write-Output 'Références, seeder, DTO et requêtes mis à jour.'
