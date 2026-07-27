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

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\ChangeGantryStatus\ChangeGantryStatusCommand.cs' `
@'
public record ChangeGantryStatusCommand(Guid GantryId, GantryStatus Statut) : IRequest;
'@ `
@'
public record ChangeGantryStatusCommand(
    Guid GantryId,
    GantryStatus Statut,
    Guid? EscaleId,
    string? TypeIncident,
    string? Cause,
    DateTime? DateDebutUtc) : IRequest;
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\ChangeGantryStatus\ChangeGantryStatusCommandHandler.cs' `
@'
using EscaleReport.Web.Domain.Identity;
'@ `
@'
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\ChangeGantryStatus\ChangeGantryStatusCommandHandler.cs' `
@'
        gantry.ChangerStatut(request.Statut);
        await dbContext.SaveChangesAsync(cancellationToken);
'@ `
@'
        var ancienStatut = gantry.Statut;
        gantry.ChangerStatut(request.Statut);

        if (request.Statut == GantryStatus.EnPanne &&
            request.EscaleId.HasValue &&
            !string.IsNullOrWhiteSpace(request.TypeIncident))
        {
            var incidentExiste = await dbContext.StsIncidents
                .AnyAsync(i => i.GantryId == request.GantryId && i.DateFinUtc == null, cancellationToken);
            if (!incidentExiste)
            {
                dbContext.StsIncidents.Add(new StsIncident
                {
                    EscaleId = request.EscaleId.Value,
                    GantryId = request.GantryId,
                    TypeIncident = request.TypeIncident.Trim(),
                    Cause = request.Cause,
                    DateDebutUtc = request.DateDebutUtc ?? DateTime.UtcNow,
                    RetirePortiqueEffectif = true
                });
            }
        }
        else if (ancienStatut == GantryStatus.EnPanne && request.Statut != GantryStatus.EnPanne)
        {
            var incidentsOuverts = await dbContext.StsIncidents
                .Where(i => i.GantryId == request.GantryId && i.DateFinUtc == null)
                .ToListAsync(cancellationToken);
            foreach (var incident in incidentsOuverts)
            {
                incident.Cloturer("Reprise automatique après remise en service");
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\AssignGantry\AssignGantryCommandHandler.cs' `
@'
            HeureDebut = request.HeureDebut,
'@ `
@'
            HeureDebut = request.HeureDebut == default ? DateTime.UtcNow : request.HeureDebut,
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\AssignGantry\AssignGantryCommandHandler.cs' `
@'
        var gantry = await dbContext.Gantries.FirstAsync(g => g.Id == request.GantryId, cancellationToken);
        gantry.ChangerStatut(GantryStatus.Affecte);
'@ `
@'
        var gantry = await dbContext.Gantries.FirstAsync(g => g.Id == request.GantryId, cancellationToken);
        if (gantry.Statut != GantryStatus.Disponible)
        {
            return Guid.Empty;
        }
        gantry.ChangerStatut(GantryStatus.Affecte);
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\AssignGantry\AssignGantryCommandValidator.cs' `
@'
                RuleFor(x => x.GantryId)
                    .MustAsync(GantryIsFreeAsync)
                    .WithMessage("Ce portique est déjà affecté à un navire en cours.");
'@ `
@'
                RuleFor(x => x.GantryId)
                    .MustAsync(GantryIsFreeAsync)
                    .WithMessage("Ce portique est déjà affecté à un navire en cours.");
                RuleFor(x => x.GantryId)
                    .MustAsync(GantryIsAvailableAsync)
                    .WithMessage("Un portique en panne, en maintenance ou déjà affecté ne peut pas être affecté.");
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\AssignGantry\AssignGantryCommandValidator.cs' `
@'
    private Task<bool> GantryExistsAsync(Guid gantryId, CancellationToken cancellationToken) =>
        _dbContext.Gantries.AnyAsync(g => g.Id == gantryId, cancellationToken);

    // Exclusivité de l'affectation
'@ `
@'
    private Task<bool> GantryExistsAsync(Guid gantryId, CancellationToken cancellationToken) =>
        _dbContext.Gantries.AnyAsync(g => g.Id == gantryId, cancellationToken);

    private Task<bool> GantryIsAvailableAsync(Guid gantryId, CancellationToken cancellationToken) =>
        _dbContext.Gantries.AnyAsync(
            g => g.Id == gantryId && g.Statut == GantryStatus.Disponible,
            cancellationToken);

    // Exclusivité de l'affectation
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\EndGantryAssignment\EndGantryAssignmentCommand.cs' `
@'
public record EndGantryAssignmentCommand(Guid AssignmentId) : IRequest;
'@ `
@'
public record EndGantryAssignmentCommand(Guid AssignmentId, DateTime? HeureFin = null) : IRequest;
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\EndGantryAssignment\EndGantryAssignmentCommandHandler.cs' `
@'
        assignment.Terminer();
'@ `
@'
        assignment.Terminer(request.HeureFin);
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\AddStsIncident\AddStsIncidentCommand.cs' `
@'
    DateTime DateDebutUtc,
    string? Cause,
'@ `
@'
    DateTime DateDebutUtc,
    DateTime? DateFinUtc,
    string? Cause,
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\AddStsIncident\AddStsIncidentCommandHandler.cs' `
@'
            DateDebutUtc = request.DateDebutUtc,
            Cause = request.Cause,
'@ `
@'
            DateDebutUtc = request.DateDebutUtc == default ? DateTime.UtcNow : request.DateDebutUtc,
            DateFinUtc = request.DateFinUtc.HasValue &&
                request.DateFinUtc.Value >= (request.DateDebutUtc == default ? DateTime.UtcNow : request.DateDebutUtc)
                    ? request.DateFinUtc
                    : null,
            Cause = request.Cause,
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\CloseStsIncident\CloseStsIncidentCommand.cs' `
@'
public record CloseStsIncidentCommand(Guid IncidentId, string? ConditionsReprise) : IRequest;
'@ `
@'
public record CloseStsIncidentCommand(
    Guid IncidentId,
    string? ConditionsReprise,
    DateTime? DateFinUtc = null) : IRequest;
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\CloseStsIncident\CloseStsIncidentCommandHandler.cs' `
@'
        incident.Cloturer(request.ConditionsReprise);
'@ `
@'
        incident.Cloturer(request.ConditionsReprise, request.DateFinUtc);
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\AddStsPointeur\AddStsPointeurCommand.cs' `
@'
    string? NavireOuZone,
    DateTime HeurePriseDePosteUtc) : IRequest<Guid>;
'@ `
@'
    string? NavireOuZone,
    DateTime HeurePriseDePosteUtc,
    DateTime? HeureFinUtc) : IRequest<Guid>;
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\AddStsPointeur\AddStsPointeurCommandHandler.cs' `
@'
            NavireOuZone = request.NavireOuZone,
            HeurePriseDePosteUtc = request.HeurePriseDePosteUtc
        };

        dbContext.StsPointeurs.Add(pointeur);
'@ `
@'
            NavireOuZone = request.NavireOuZone,
            HeurePriseDePosteUtc = request.HeurePriseDePosteUtc == default
                ? DateTime.UtcNow
                : request.HeurePriseDePosteUtc
        };
        if (request.HeureFinUtc.HasValue)
        {
            pointeur.TerminerService(request.HeureFinUtc);
        }

        dbContext.StsPointeurs.Add(pointeur);
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\EndStsPointeur\EndStsPointeurCommand.cs' `
@'
public record EndStsPointeurCommand(Guid PointeurId) : IRequest;
'@ `
@'
public record EndStsPointeurCommand(Guid PointeurId, DateTime? HeureFinUtc = null) : IRequest;
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\EndStsPointeur\EndStsPointeurCommandHandler.cs' `
@'
        pointeur.TerminerService();
'@ `
@'
        pointeur.TerminerService(request.HeureFinUtc);
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Dtos\StsPointeurDto.cs' `
@'
    public string? Remarque { get; set; }

    public static StsPointeurDto FromEntity
'@ `
@'
    public string? Remarque { get; set; }
    public TimeSpan? Duree { get; set; }

    public static StsPointeurDto FromEntity
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Dtos\StsPointeurDto.cs' `
@'
        HeureFinUtc = p.HeureFinUtc,
        Remarque = p.Remarque
'@ `
@'
        HeureFinUtc = p.HeureFinUtc,
        Remarque = p.Remarque,
        Duree = p.Duree
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\AddRopnEntry\AddRopnEntryCommand.cs' `
@'
public record AddRopnEntryCommand(string Nom, string? Role, string DifficulteRencontree) : IRequest<Guid>;
'@ `
@'
public record AddRopnEntryCommand(
    string Nom,
    string? Role,
    DateTime DateDebutUtc,
    DateTime? DateFinUtc,
    string DifficulteRencontree) : IRequest<Guid>;
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\AddRopnEntry\AddRopnEntryCommandHandler.cs' `
@'
            Nom = request.Nom,
            Role = request.Role,
            DifficulteRencontree = request.DifficulteRencontree
'@ `
@'
            Nom = request.Nom,
            Role = request.Role,
            DateDebutUtc = request.DateDebutUtc == default ? DateTime.UtcNow : request.DateDebutUtc,
            DateFinUtc = request.DateFinUtc,
            DifficulteRencontree = request.DifficulteRencontree
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\ResolveRopnEntry\ResolveRopnEntryCommand.cs' `
@'
public record ResolveRopnEntryCommand(Guid RopnEntryId, string? ActionRealisee) : IRequest;
'@ `
@'
public record ResolveRopnEntryCommand(
    Guid RopnEntryId,
    string? ActionRealisee,
    DateTime? DateFinUtc = null) : IRequest;
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Commands\ResolveRopnEntry\ResolveRopnEntryCommandHandler.cs' `
@'
        entry.Resoudre(request.ActionRealisee);
'@ `
@'
        entry.Resoudre(request.ActionRealisee, request.DateFinUtc);
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Dtos\RopnEntryDto.cs' `
@'
    public string? Role { get; set; }
    public string DifficulteRencontree
'@ `
@'
    public string? Role { get; set; }
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string DifficulteRencontree
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Dtos\RopnEntryDto.cs' `
@'
        Role = r.Role,
        DifficulteRencontree = r.DifficulteRencontree,
'@ `
@'
        Role = r.Role,
        DateDebutUtc = r.DateDebutUtc,
        DateFinUtc = r.DateFinUtc,
        Duree = r.Duree,
        DifficulteRencontree = r.DifficulteRencontree,
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Dtos\GantryAssignmentDto.cs' `
@'
    public DateTime? HeureFin { get; set; }
    public string? TacheOuZone
'@ `
@'
    public DateTime? HeureFin { get; set; }
    public TimeSpan? Duree { get; set; }
    public string? TacheOuZone
'@

Replace-Normalized `
    '.\EscaleReport.Web\Application\Dispatch\Dtos\StsIncidentDto.cs' `
@'
    public Guid Id { get; set; }
    public string Navire
'@ `
@'
    public Guid Id { get; set; }
    public Guid EscaleId { get; set; }
    public Guid? GantryId { get; set; }
    public string Navire
'@

$newHandlers = @(
    '.\EscaleReport.Web\Application\Dispatch\Commands\UpdateGantryAssignment\UpdateGantryAssignmentCommandHandler.cs',
    '.\EscaleReport.Web\Application\Dispatch\Commands\UpdateStsIncident\UpdateStsIncidentCommandHandler.cs'
)
foreach ($handlerPath in $newHandlers) {
    $resolved = (Resolve-Path -LiteralPath $handlerPath).Path
    $content = [IO.File]::ReadAllText($resolved).Replace("`r`n", "`n")
    if (-not $content.Contains('using EscaleReport.Web.Application.Dispatch;')) {
        $content = $content.Replace(
            'using EscaleReport.Web.Application.Common.Interfaces;',
            "using EscaleReport.Web.Application.Common.Interfaces;`nusing EscaleReport.Web.Application.Dispatch;")
    }
    [IO.File]::WriteAllText($resolved, $content, [Text.UTF8Encoding]::new($false))
}

Write-Output 'Commandes et DTO STS vague 4 mis à jour.'
