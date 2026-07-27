using FluentValidation;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddStsIncident;

public class AddStsIncidentCommandValidator : AbstractValidator<AddStsIncidentCommand>
{
    public AddStsIncidentCommandValidator()
    {
        RuleFor(x => x.EscaleId).NotEmpty();
        RuleFor(x => x.TypeIncident).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Cause).MaximumLength(1000);
        RuleFor(x => x)
            .Must(x => !x.DateFinUtc.HasValue ||
                       x.DateDebutUtc == default ||
                       x.DateFinUtc.Value >= x.DateDebutUtc)
            .WithMessage("La fin de l’incident doit être postérieure ou égale à son début.");
    }
}
