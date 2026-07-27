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

Update-ExactFile -Path 'EscaleReport.Web\Application\VesselPlanning\Commands\AddOperationalIncident\AddOperationalIncidentCommand.cs' -Replacements @(
    @{
        Old = @'
    string? Localisation,
    DateTime DateDebutUtc,
    IncidentGravite Gravite,
'@
        New = @'
    string? Localisation,
    DateTime DateDebutUtc,
    DateTime? DateFinUtc,
    IncidentGravite Gravite,
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Application\VesselPlanning\Commands\AddOperationalIncident\AddOperationalIncidentCommandHandler.cs' -Replacements @(
    @{
        Old = @'
            DateDebutUtc = request.DateDebutUtc,
            Gravite = request.Gravite,
'@
        New = @'
            DateDebutUtc = request.DateDebutUtc,
            DateFinUtc = request.DateFinUtc,
            Statut = request.DateFinUtc.HasValue ? IncidentStatus.Resolu : IncidentStatus.EnCours,
            Gravite = request.Gravite,
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Domain\VesselPlanning\OperationalIncident.cs' -Replacements @(
    @{
        Old = @'
        Statut = IncidentStatus.Resolu;
        DateFinUtc = DateTime.UtcNow;
'@
        New = @'
        Statut = IncidentStatus.Resolu;
        var now = DateTime.UtcNow;
        DateFinUtc = now < DateDebutUtc ? DateDebutUtc : now;
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Application\VesselPlanning\Commands\AddDangerousContainer\AddDangerousContainerCommand.cs' -Replacements @(
    @{
        Old = @'
    string? ClasseImo,
    string? Position,
    DateTime? DateValiditeBadt) : IRequest<Guid>;
'@
        New = @'
    string? ClasseImo,
    string? Position,
    DateTime? DateValiditeBadt,
    BadtStatus StatutBadt = BadtStatus.NonPris,
    DangerousContainerStatus StatutOperationnel = DangerousContainerStatus.ASuivre) : IRequest<Guid>;
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Application\VesselPlanning\Commands\AddDangerousContainer\AddDangerousContainerCommandHandler.cs' -Replacements @(
    @{
        Old = @'
            Position = request.Position,
            DateValiditeBadt = request.DateValiditeBadt
'@
        New = @'
            Position = request.Position,
            DateValiditeBadt = request.DateValiditeBadt,
            StatutBadt = request.StatutBadt,
            StatutOperationnel = request.StatutOperationnel
'@
    }
)

Update-ExactFile -Path 'EscaleReport.Web\Application\VesselPlanning\Commands\UpdateEmptyContainerTarget\UpdateEmptyContainerTargetCommandHandler.cs' -Replacements @(
    @{
        Old = @'
        var target = await dbContext.EmptyContainerTargets
'@
        New = @'
        if (request.QuantiteAjoutee < 0 || request.QuantitePlanifiee < 0 ||
            request.QuantiteEmbarquee < 0 || request.QuantiteCoupee < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "Les quantités ne peuvent pas être négatives.");
        }

        if (request.QuantiteCoupee > 0 && string.IsNullOrWhiteSpace(request.MotifCoupure))
        {
            throw new ArgumentException("Le motif de coupure est obligatoire.", nameof(request));
        }

        var target = await dbContext.EmptyContainerTargets
'@
    }
)

Write-Output 'Commandes, handlers et domaine Vessel Planning mis à jour.'
