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
            throw "Bloc attendu introuvable dans $Path : $($old.Substring(0, [Math]::Min(100, $old.Length)))"
        }
        $text = $text.Replace($old, $new)
    }
    [IO.File]::WriteAllText($resolved, $text.Replace("`n", [Environment]::NewLine), [Text.UTF8Encoding]::new($false))
}

$pdf = 'EscaleReport.Web\Infrastructure\Pdf\QuestPdfEscaleReportGenerator.cs'
Update-ExactFile -Path $pdf -Replacements @(
    @{
        Old = '            SectionTitle(col, "Conteneurs en anomalie");'
        New = '            SectionTitle(col, "1. Conteneurs en anomalie");'
    },
    @{
        Old = '            SectionTitle(col, "Conteneurs vides");'
        New = '            SectionTitle(col, "2. Conteneurs vides");'
    },
    @{
        Old = @'
                Table(col, ["Ligne", "Type", "Souhaitée", "Planifiée", "Embarquée", "Coupée", "Restante"], detail.ConteneursVides.Items.Select(v => new[]
                {
                    v.LigneMaritime,
                    v.TypeConteneur,
                    v.QuantiteSouhaitee.ToString(),
                    v.QuantitePlanifiee.ToString(),
                    v.QuantiteEmbarquee.ToString(),
                    v.QuantiteCoupee.ToString(),
                    v.QuantiteRestante.ToString()
                }));
'@
        New = @'
                Table(col, ["Ligne", "Type", "Souhaitée", "Ajoutée", "Planifiée", "Embarquée", "Coupée", "Restante"], detail.ConteneursVides.Items.Select(v => new[]
                {
                    v.LigneMaritime,
                    v.TypeConteneur,
                    v.QuantiteSouhaitee.ToString(),
                    v.QuantiteAjoutee.ToString(),
                    v.QuantitePlanifiee.ToString(),
                    v.QuantiteEmbarquee.ToString(),
                    v.QuantiteCoupee.ToString(),
                    v.QuantiteRestante.ToString()
                }));
                col.Item().PaddingTop(5)
                    .Text($"Totaux — souhaitée {detail.VidesSouhaiteTotal}, ajoutée {detail.VidesAjouteTotal}, planifiée {detail.VidesPlanifieTotal}, embarquée {detail.VidesEmbarqueTotal}, coupée {detail.VidesCoupeTotal}, restante {detail.VidesResteTotal}")
                    .FontSize(8).Bold();
'@
    },
    @{
        Old = '            SectionTitle(col, "Incidents opérationnels");'
        New = '            SectionTitle(col, "3. Incidents opérationnels");'
    },
    @{
        Old = @'
                Table(col, ["Catégorie", "Localisation", "Début", "Statut", "Description"], detail.Incidents.Items.Select(i => new[]
                {
                    i.Categorie,
                    i.Localisation ?? "—",
                    i.DateDebutUtc.ToString("dd/MM HH:mm"),
                    i.Statut == IncidentStatus.Resolu ? "Résolu" : "En cours",
                    i.Description ?? "—"
                }));
'@
        New = @'
                Table(col, ["Catégorie", "Localisation", "Début", "Fin", "Durée", "Gravité", "Statut", "Description"], detail.Incidents.Items.Select(i => new[]
                {
                    i.Categorie,
                    i.Localisation ?? "—",
                    i.DateDebutUtc.ToString("dd/MM HH:mm"),
                    i.DateFinUtc?.ToString("dd/MM HH:mm") ?? "—",
                    i.Duree.HasValue ? $"{(int)i.Duree.Value.TotalHours}h{i.Duree.Value.Minutes:D2}" : "—",
                    i.Gravite.ToString(),
                    i.Statut == IncidentStatus.Resolu ? "Résolu" : "En cours",
                    i.Description ?? "—"
                }));
'@
    },
    @{
        Old = '            SectionTitle(col, "Incidents STS");'
        New = '            SectionTitle(col, "3.1 Incidents STS");'
    },
    @{
        Old = '            SectionTitle(col, "Conteneurs additionnels");'
        New = '            SectionTitle(col, "4. Conteneurs additionnels");'
    },
    @{
        Old = '            SectionTitle(col, "Conteneurs dangereux");'
        New = '            SectionTitle(col, "5. Conteneurs dangereux");'
    },
    @{
        Old = @'
                Table(col, ["Conteneur", "Classe IMO", "Statut BADT", "Statut opérationnel"], detail.ConteneursDangereux.Items.Select(c => new[]
                {
                    c.NumeroConteneur,
                    c.ClasseImo ?? "—",
                    BadtLabel(c.StatutBadt),
                    OperationnelLabel(c.StatutOperationnel)
                }));
'@
        New = @'
                Table(col, ["Conteneur", "Classe IMO", "Statut BADT", "Statut opérationnel"], detail.ConteneursDangereux.Items.Select(c => new[]
                {
                    c.NumeroConteneur,
                    c.ClasseImo ?? "—",
                    BadtLabel(c.StatutBadt),
                    OperationnelLabel(c.StatutOperationnel)
                }), row => row[2].Contains("renouveler", StringComparison.OrdinalIgnoreCase));
'@
    },
    @{
        Old = @'
        BadtStatus.ARenouveler => "À renouveler",
'@
        New = @'
        BadtStatus.ARenouveler => "⚠ À renouveler",
'@
    },
    @{
        Old = @'
    private static void Table(ColumnDescriptor col, string[] headers, IEnumerable<string[]> rows)
'@
        New = @'
    private static void Table(
        ColumnDescriptor col,
        string[] headers,
        IEnumerable<string[]> rows,
        Func<string[], bool>? highlightRow = null)
'@
    },
    @{
        Old = @'
            foreach (var row in rows)
            {
                foreach (var cell in row)
                {
                    BodyCell(table, cell);
                }
            }
'@
        New = @'
            foreach (var row in rows)
            {
                var highlighted = highlightRow?.Invoke(row) == true;
                foreach (var cell in row)
                {
                    BodyCell(table, cell, highlighted);
                }
            }
'@
    },
    @{
        Old = @'
    private static void BodyCell(TableDescriptor table, string text) =>
        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(text).FontSize(9);
'@
        New = @'
    private static void BodyCell(TableDescriptor table, string text, bool highlighted = false) =>
        table.Cell()
            .Background(highlighted ? Colors.Red.Lighten4 : Colors.White)
            .BorderBottom(0.5f)
            .BorderColor(highlighted ? Colors.Red.Lighten2 : Colors.Grey.Lighten2)
            .Padding(5)
            .Text(text)
            .FontSize(9)
            .FontColor(highlighted ? Colors.Red.Darken2 : Colors.Grey.Darken4);
'@
    }
)

Write-Output 'Rapport PDF enrichi et sections numerotees.'
