using FluentValidation;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateRtgEffectif;

public class UpdateRtgEffectifCommandValidator : AbstractValidator<UpdateRtgEffectifCommand>
{
    public UpdateRtgEffectifCommandValidator()
    {
        RuleFor(x => x.TotalParc).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Disponible).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Affecte).GreaterThanOrEqualTo(0);
        RuleFor(x => x.EnPanne).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Retire).GreaterThanOrEqualTo(0);
        RuleFor(x => x)
            .Must(x => x.Disponible + x.Affecte + x.EnPanne + x.Retire <= x.TotalParc)
            .WithMessage("La somme disponible + affecté + en panne + retiré ne peut pas dépasser le total du parc.")
            .WithName("TotalParc");
    }
}
