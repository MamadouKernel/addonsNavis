$ErrorActionPreference = 'Stop'

function Replace-Exact {
    param(
        [Parameter(Mandatory)] [string] $Path,
        [Parameter(Mandatory)] [string] $Old,
        [Parameter(Mandatory)] [string] $New
    )

    $resolved = (Resolve-Path -LiteralPath $Path).Path
    $content = [IO.File]::ReadAllText($resolved)
    if (-not $content.Contains($Old)) {
        throw "Bloc introuvable dans $Path"
    }

    [IO.File]::WriteAllText($resolved, $content.Replace($Old, $New), [Text.UTF8Encoding]::new($false))
}

Replace-Exact `
    '.\EscaleReport.Web\Domain\Dispatch\GantryAssignment.cs' `
@'
    public string? TacheOuZone { get; set; }
    public AssignmentStatus Statut { get; set; } = AssignmentStatus.EnCours;

    public void Terminer()
    {
        Statut = AssignmentStatus.Terminee;
        HeureFin ??= DateTime.UtcNow;
    }
'@ `
@'
    public string? TacheOuZone { get; set; }
    public AssignmentStatus Statut { get; set; } = AssignmentStatus.EnCours;
    public TimeSpan? Duree => HeureFin.HasValue ? HeureFin.Value - HeureDebut : null;

    public void CorrigerHeureDebut(DateTime heureDebut)
    {
        HeureDebut = heureDebut;
        if (HeureFin.HasValue && HeureFin.Value < HeureDebut)
        {
            HeureFin = null;
            Statut = AssignmentStatus.EnCours;
        }
    }

    public void Terminer(DateTime? heureFin = null)
    {
        Statut = AssignmentStatus.Terminee;
        var fin = heureFin ?? DateTime.UtcNow;
        HeureFin = fin < HeureDebut ? HeureDebut : fin;
    }
'@

Replace-Exact `
    '.\EscaleReport.Web\Domain\Dispatch\StsPointeur.cs' `
@'
    public string? Remarque { get; set; }

    public void TerminerService()
    {
        HeureFinUtc = DateTime.UtcNow;
    }
'@ `
@'
    public string? Remarque { get; set; }
    public TimeSpan? Duree => HeureFinUtc.HasValue ? HeureFinUtc.Value - HeurePriseDePosteUtc : null;

    public void TerminerService(DateTime? heureFinUtc = null)
    {
        var fin = heureFinUtc ?? DateTime.UtcNow;
        HeureFinUtc = fin < HeurePriseDePosteUtc ? HeurePriseDePosteUtc : fin;
    }
'@

Replace-Exact `
    '.\EscaleReport.Web\Domain\Dispatch\RopnEntry.cs' `
@'
    public string? Role { get; set; }
    public string DifficulteRencontree { get; set; } = string.Empty;
    public string? ActionRealisee { get; set; }
    public RopnStatus Statut { get; set; } = RopnStatus.EnCours;
    public string? Commentaire { get; set; }

    public void Resoudre(string? actionRealisee)
    {
        Statut = RopnStatus.Resolu;
        if (!string.IsNullOrWhiteSpace(actionRealisee))
        {
            ActionRealisee = actionRealisee;
        }
    }
'@ `
@'
    public string? Role { get; set; }
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public string DifficulteRencontree { get; set; } = string.Empty;
    public string? ActionRealisee { get; set; }
    public RopnStatus Statut { get; set; } = RopnStatus.EnCours;
    public string? Commentaire { get; set; }
    public TimeSpan? Duree => DateFinUtc.HasValue ? DateFinUtc.Value - DateDebutUtc : null;

    public void Resoudre(string? actionRealisee, DateTime? dateFinUtc = null)
    {
        Statut = RopnStatus.Resolu;
        var fin = dateFinUtc ?? DateTime.UtcNow;
        DateFinUtc = fin < DateDebutUtc ? DateDebutUtc : fin;
        if (!string.IsNullOrWhiteSpace(actionRealisee))
        {
            ActionRealisee = actionRealisee;
        }
    }
'@

Replace-Exact `
    '.\EscaleReport.Web\Domain\Dispatch\StsIncident.cs' `
@'
    public void Cloturer(string? conditionsReprise)
    {
        DateFinUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(conditionsReprise))
        {
            ConditionsReprise = conditionsReprise;
        }
    }
'@ `
@'
    public void MettreAJour(
        Guid escaleId,
        Guid? gantryId,
        string typeIncident,
        DateTime dateDebutUtc,
        DateTime? dateFinUtc,
        string? cause,
        string? conditionsReprise,
        bool retirePortiqueEffectif)
    {
        EscaleId = escaleId;
        GantryId = gantryId;
        TypeIncident = typeIncident.Trim();
        DateDebutUtc = dateDebutUtc;
        DateFinUtc = dateFinUtc.HasValue && dateFinUtc.Value >= dateDebutUtc ? dateFinUtc : null;
        Cause = cause;
        ConditionsReprise = conditionsReprise;
        RetirePortiqueEffectif = retirePortiqueEffectif;
    }

    public void Cloturer(string? conditionsReprise, DateTime? dateFinUtc = null)
    {
        var fin = dateFinUtc ?? DateTime.UtcNow;
        DateFinUtc = fin < DateDebutUtc ? DateDebutUtc : fin;
        if (!string.IsNullOrWhiteSpace(conditionsReprise))
        {
            ConditionsReprise = conditionsReprise;
        }
    }
'@

Replace-Exact `
    '.\EscaleReport.Web\Domain\Common\ReferenceValue.cs' `
@'
    public const string StsIncidentType = "StsIncidentType";
    public const string YardZone = "YardZone";
'@ `
@'
    public const string StsIncidentType = "StsIncidentType";
    public const string StsVesselIncidentType = "StsVesselIncidentType";
    public const string YardZone = "YardZone";
'@

Replace-Exact `
    '.\EscaleReport.Web\Domain\Common\ReferenceValue.cs' `
@'
        [StsIncidentType] = "Types de panne STS",
        [YardZone] = "Zones Yard",
'@ `
@'
        [StsIncidentType] = "Types de panne portique STS",
        [StsVesselIncidentType] = "Types d'incident navire STS",
        [YardZone] = "Zones Yard",
'@

Write-Output 'Domaine STS vague 4 mis à jour.'
