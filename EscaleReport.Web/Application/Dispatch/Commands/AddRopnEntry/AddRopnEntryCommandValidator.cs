using FluentValidation;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddRopnEntry;

public class AddRopnEntryCommandValidator : AbstractValidator<AddRopnEntryCommand>
{
    public AddRopnEntryCommandValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DifficulteRencontree).NotEmpty().MaximumLength(1000);
        RuleFor(x => x)
            .Must(x => !x.DateFinUtc.HasValue ||
                       x.DateDebutUtc == default ||
                       x.DateFinUtc.Value >= x.DateDebutUtc)
            .WithMessage("La fin ROPN doit être postérieure ou égale au début.");
    }
}
