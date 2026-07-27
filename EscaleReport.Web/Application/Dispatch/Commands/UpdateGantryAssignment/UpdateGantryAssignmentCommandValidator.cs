using FluentValidation;

namespace EscaleReport.Web.Application.Dispatch.Commands.UpdateGantryAssignment;

public class UpdateGantryAssignmentCommandValidator : AbstractValidator<UpdateGantryAssignmentCommand>
{
    public UpdateGantryAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentId).NotEmpty();
        RuleFor(x => x.HeureDebut).NotEmpty();
        RuleFor(x => x)
            .Must(x => !x.HeureFin.HasValue || x.HeureFin.Value >= x.HeureDebut)
            .WithMessage("L’heure de fin doit être postérieure ou égale à l’heure de début.");
    }
}
