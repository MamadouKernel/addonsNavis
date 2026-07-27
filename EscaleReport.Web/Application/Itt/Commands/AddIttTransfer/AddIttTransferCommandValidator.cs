using EscaleReport.Web.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Itt.Commands.AddIttTransfer;

public sealed class AddIttTransferCommandValidator : AbstractValidator<AddIttTransferCommand>
{
    public AddIttTransferCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.NavireConnexion)
            .NotEmpty().WithMessage("Sélectionnez un navire de connexion.")
            .MustAsync((navire, token) => dbContext.Escales.AnyAsync(e => e.Navire == navire, token))
            .WithMessage("Sélectionnez un navire existant dans les escales.");
        RuleFor(x => x.NombreATransferer).GreaterThanOrEqualTo(0);
        RuleFor(x => x.NombreTransfere).GreaterThanOrEqualTo(0);
        RuleFor(x => x.NombreRecu).GreaterThanOrEqualTo(0);
    }
}
