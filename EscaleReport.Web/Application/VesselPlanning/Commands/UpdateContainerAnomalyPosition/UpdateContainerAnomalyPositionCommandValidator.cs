using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.UpdateContainerAnomalyPosition;

public sealed class UpdateContainerAnomalyPositionCommandValidator : AbstractValidator<UpdateContainerAnomalyPositionCommand>
{
    public UpdateContainerAnomalyPositionCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Position).MustAsync(async (position, token) =>
            string.IsNullOrWhiteSpace(position) || await dbContext.ReferenceValues.AnyAsync(
                r => r.ListKey == ReferenceListKeys.Bay && r.Value == position && r.IsActive, token))
            .WithMessage("Sélectionnez un bay actif dans la liste administrée.");
    }
}
