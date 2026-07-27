using FluentValidation;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddStsPointeur;

public class AddStsPointeurCommandValidator : AbstractValidator<AddStsPointeurCommand>
{
    public AddStsPointeurCommandValidator()
    {
        RuleFor(x => x.Nom).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Role)
            .Must(role => string.IsNullOrWhiteSpace(role) ||
                          role.Equals("Terre", StringComparison.OrdinalIgnoreCase) ||
                          role.Equals("Bord", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Le rôle du pointeur doit être Terre ou Bord.");
        RuleFor(x => x)
            .Must(x => !x.HeureFinUtc.HasValue ||
                       x.HeurePriseDePosteUtc == default ||
                       x.HeureFinUtc.Value >= x.HeurePriseDePosteUtc)
            .WithMessage("La fin de service doit être postérieure ou égale à la prise de poste.");
    }
}
