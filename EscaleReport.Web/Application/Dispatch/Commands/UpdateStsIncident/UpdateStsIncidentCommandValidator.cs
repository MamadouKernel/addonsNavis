using FluentValidation;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateStsIncident;

public class UpdateStsIncidentCommandValidator : AbstractValidator<UpdateStsIncidentCommand>
{
    public UpdateStsIncidentCommandValidator()
    {
        RuleFor(x => x.IncidentId).NotEmpty();
        RuleFor(x => x.EscaleId).NotEmpty();
        RuleFor(x => x.TypeIncident).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DateDebutUtc).NotEmpty();
        RuleFor(x => x)
            .Must(x => !x.DateFinUtc.HasValue || x.DateFinUtc.Value >= x.DateDebutUtc)
            .WithMessage("La fin de l’incident doit être postérieure ou égale à son début.");
        RuleFor(x => x.Cause).MaximumLength(1000);
        RuleFor(x => x.ConditionsReprise).MaximumLength(1000);
    }
}
