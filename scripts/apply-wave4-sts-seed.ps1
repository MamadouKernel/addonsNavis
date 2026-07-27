$ErrorActionPreference = 'Stop'

$path = (Resolve-Path -LiteralPath '.\EscaleReport.Web\Infrastructure\Persistence\DbSeeder.cs').Path
$content = [IO.File]::ReadAllText($path)

if (-not $content.Contains('SeedStsVesselIncidentTypesAsync(dbContext)')) {
    $content = $content.Replace(
        '        await SeedStsIncidentTypesAsync(dbContext);',
        "        await SeedStsIncidentTypesAsync(dbContext);`r`n        await SeedStsVesselIncidentTypesAsync(dbContext);")
}

if (-not $content.Contains('private static async Task SeedStsVesselIncidentTypesAsync')) {
    $marker = '    private static async Task SeedGantriesAsync(IApplicationDbContext dbContext)'
    $method = @'
    private static async Task SeedStsVesselIncidentTypesAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.ReferenceValues.AnyAsync(r => r.ListKey == ReferenceListKeys.StsVesselIncidentType))
        {
            return;
        }

        string[] types =
        [
            "Attente documents navire", "Attente équipage", "Arrêt opérations navire",
            "Problème de plan de chargement", "Problème de communication navire", "Autre incident navire"
        ];
        for (var i = 0; i < types.Length; i++)
        {
            dbContext.ReferenceValues.Add(new ReferenceValue
            {
                ListKey = ReferenceListKeys.StsVesselIncidentType,
                Value = types[i],
                SortOrder = i
            });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

'@
    if (-not $content.Contains($marker)) {
        throw "Point insertion SeedGantriesAsync introuvable."
    }
    $content = $content.Replace($marker, $method + $marker)
}

[IO.File]::WriteAllText($path, $content, [Text.UTF8Encoding]::new($false))
Write-Output 'Seed des incidents navire STS ajouté.'
