using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddContainerAnomaly;

public class AddContainerAnomalyCommandValidator : AbstractValidator<AddContainerAnomalyCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public AddContainerAnomalyCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.EscaleId)
            .MustAsync(EscaleExistsAsync).WithMessage("Escale introuvable.");

        RuleFor(x => x.NumeroConteneur)
            .NotEmpty().WithMessage("Le numéro de conteneur est obligatoire.")
            .MaximumLength(50);

        RuleFor(x => x.Raison)
            .NotEmpty().WithMessage("La raison de l'anomalie est obligatoire.")
            .MustAsync(BeAKnownReasonAsync)
            .WithMessage("Raison inconnue : elle doit correspondre à une valeur paramétrée (Réglages > Raisons d'anomalie).");

        RuleFor(x => x.Position).MustAsync(async (position, token) =>
            string.IsNullOrWhiteSpace(position) || await _dbContext.ReferenceValues.AnyAsync(
                r => r.ListKey == ReferenceListKeys.Bay && r.Value == position && r.IsActive, token))
            .WithMessage("Sélectionnez un bay actif dans la liste administrée.");
    }

    private Task<bool> EscaleExistsAsync(Guid escaleId, CancellationToken cancellationToken) =>
        _dbContext.Escales.AnyAsync(e => e.Id == escaleId, cancellationToken);

    // CDC §27 : les raisons d'anomalie sont une liste paramétrable, pas un enum figé dans le
    // code — on valide donc contre ReferenceValue plutôt que contre des constantes C#.
    private Task<bool> BeAKnownReasonAsync(string raison, CancellationToken cancellationToken) =>
        _dbContext.ReferenceValues.AnyAsync(
            r => r.ListKey == ReferenceListKeys.AnomalyReason && r.Value == raison && r.IsActive,
            cancellationToken);
}
