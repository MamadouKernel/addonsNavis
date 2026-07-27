using EscaleReport.Web.Domain.Dispatch;
using FluentValidation;

namespace EscaleReport.Web.Application.Dispatch.Commands.ChangeGantryStatus;

public class ChangeGantryStatusCommandValidator : AbstractValidator<ChangeGantryStatusCommand>
{
    public ChangeGantryStatusCommandValidator()
    {
        RuleFor(x => x.GantryId).NotEmpty();
        When(x => x.Statut == GantryStatus.EnPanne, () =>
        {
            RuleFor(x => x.EscaleId)
                .NotEmpty()
                .WithMessage("Sélectionnez le navire concerné par la panne.");
            RuleFor(x => x.TypeIncident)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Le type de panne est obligatoire.");
            RuleFor(x => x.Cause).MaximumLength(1000);
        });
    }
}
