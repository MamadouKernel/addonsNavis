using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Dispatch;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.AssignGantry;

public class AssignGantryCommandValidator : AbstractValidator<AssignGantryCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public AssignGantryCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.GantryId)
            .MustAsync(GantryExistsAsync).WithMessage("Portique introuvable.")
            .DependentRules(() =>
            {
                RuleFor(x => x.GantryId)
                    .MustAsync(GantryIsFreeAsync)
                    .WithMessage("Ce portique est déjà affecté à un navire en cours.");
            });

        RuleFor(x => x.EscaleId)
            .MustAsync((escaleId, ct) => _dbContext.Escales.AnyAsync(e => e.Id == escaleId, ct))
            .WithMessage("Escale introuvable.");
    }

    private Task<bool> GantryExistsAsync(Guid gantryId, CancellationToken cancellationToken) =>
        _dbContext.Gantries.AnyAsync(g => g.Id == gantryId, cancellationToken);

    // Exclusivité de l'affectation : un portique physique ne peut travailler qu'un seul
    // navire à la fois — contrainte métier propre à ce pattern d'allocation de ressource.
    private async Task<bool> GantryIsFreeAsync(Guid gantryId, CancellationToken cancellationToken)
    {
        var hasActiveAssignment = await _dbContext.GantryAssignments
            .Where(a => a.GantryId == gantryId && a.Statut == AssignmentStatus.EnCours)
            .AnyAsync(cancellationToken);
        return !hasActiveAssignment;
    }
}
