using FluentValidation;

namespace EscaleReport.Web.Application.Escales.Commands.CreateEscale;

public class CreateEscaleCommandValidator : AbstractValidator<CreateEscaleCommand>
{
    public CreateEscaleCommandValidator()
    {
        RuleFor(x => x.Navire)
            .NotEmpty().WithMessage("Le nom du navire est obligatoire pour créer une fiche escale.")
            .MaximumLength(200);

        RuleFor(x => x.Voyage).MaximumLength(100);
        RuleFor(x => x.LigneMaritime).MaximumLength(200);
        RuleFor(x => x.VesselVisit).MaximumLength(100);
        RuleFor(x => x.Quai).MaximumLength(100);
    }
}
