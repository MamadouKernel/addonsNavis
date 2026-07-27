using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.YardPlanning.Commands.AddTransfertOut;

public sealed class AddTransfertOutCommandValidator : AbstractValidator<AddTransfertOutCommand>
{
    public AddTransfertOutCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Bay)
            .NotEmpty().WithMessage("Sélectionnez un bay.")
            .MustAsync((bay, token) => dbContext.ReferenceValues.AnyAsync(
                r => r.ListKey == ReferenceListKeys.Bay && r.Value == bay && r.IsActive, token))
            .WithMessage("Ce bay n’existe pas ou a été désactivé par l’administrateur.");
        RuleFor(x => x.NombreConteneurs).GreaterThanOrEqualTo(0);
    }
}
