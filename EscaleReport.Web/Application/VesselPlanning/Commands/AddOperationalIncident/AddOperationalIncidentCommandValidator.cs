using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddOperationalIncident;

public class AddOperationalIncidentCommandValidator : AbstractValidator<AddOperationalIncidentCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public AddOperationalIncidentCommandValidator(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.EscaleId)
            .MustAsync(EscaleExistsAsync)
            .WithMessage("Escale introuvable.");

        RuleFor(x => x.Categorie)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(BeAnActiveCategoryAsync)
            .WithMessage("La catégorie doit correspondre à une valeur active du paramétrage.");

        RuleFor(x => x.DateDebutUtc)
            .NotEmpty()
            .WithMessage("L'heure de début est obligatoire.");

        RuleFor(x => x.DateFinUtc)
            .Must((command, dateFin) => !dateFin.HasValue || dateFin.Value >= command.DateDebutUtc)
            .WithMessage("L'heure de fin ne peut pas être antérieure à l'heure de début.");

        RuleFor(x => x.Gravite)
            .IsInEnum()
            .MustAsync(BeAnActiveSeverityAsync)
            .WithMessage("La gravité doit correspondre à une valeur active du paramétrage.");

        RuleFor(x => x.Localisation).MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
    }

    private Task<bool> EscaleExistsAsync(Guid escaleId, CancellationToken cancellationToken) =>
        _dbContext.Escales.AnyAsync(e => e.Id == escaleId, cancellationToken);

    private Task<bool> BeAnActiveCategoryAsync(string category, CancellationToken cancellationToken) =>
        _dbContext.ReferenceValues.AnyAsync(
            r => r.ListKey == ReferenceListKeys.IncidentCategory &&
                 r.Value == category &&
                 r.IsActive,
            cancellationToken);

    private Task<bool> BeAnActiveSeverityAsync(
        Domain.VesselPlanning.IncidentGravite severity,
        CancellationToken cancellationToken) =>
        _dbContext.ReferenceValues.AnyAsync(
            r => r.ListKey == ReferenceListKeys.IncidentSeverity &&
                 r.Value == severity.ToString() &&
                 r.IsActive,
            cancellationToken);
}
