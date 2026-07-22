using FluentValidation;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateTtEffectif;

public class UpdateTtEffectifCommandValidator : AbstractValidator<UpdateTtEffectifCommand>
{
    public UpdateTtEffectifCommandValidator()
    {
        RuleFor(x => x.TotalParc).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Designes)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.TotalParc)
            .WithMessage("Le nombre de TT désignés ne peut pas dépasser le total du parc.");
        RuleFor(x => x.Retires)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.Designes)
            .WithMessage("Le nombre de TT retirés ne peut pas dépasser le nombre de TT désignés.");
    }
}
