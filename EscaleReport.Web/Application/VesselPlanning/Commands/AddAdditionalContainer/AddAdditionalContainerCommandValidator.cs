using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddAdditionalContainer;

public sealed class AddAdditionalContainerCommandValidator : AbstractValidator<AddAdditionalContainerCommand>
{
    public AddAdditionalContainerCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Position).MustAsync(async (position, token) =>
            string.IsNullOrWhiteSpace(position) || await dbContext.ReferenceValues.AnyAsync(
                r => r.ListKey == ReferenceListKeys.Bay && r.Value == position && r.IsActive, token))
            .WithMessage("Sélectionnez un bay actif dans la liste administrée.");
    }
}
