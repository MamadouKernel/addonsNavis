using EscaleReport.Web.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Commands.AssignTt;

public class AssignTtCommandValidator : AbstractValidator<AssignTtCommand>
{
    public AssignTtCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.EscaleId)
            .MustAsync((id, ct) => dbContext.Escales.AnyAsync(e => e.Id == id, ct))
            .WithMessage("Escale introuvable.");

        RuleFor(x => x.NombrePrevu).GreaterThanOrEqualTo(0);
        RuleFor(x => x.NombreAffecte).GreaterThanOrEqualTo(0);
        RuleFor(x => x.NombreOperationnel)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.NombreAffecte)
            .WithMessage("Le nombre opérationnel ne peut pas dépasser le nombre affecté.");

        RuleFor(x => x.Observations)
            .MaximumLength(1000)
            .Must(BeSafeForSpreadsheetExport)
            .WithMessage("L'observation ne peut pas commencer par =, +, - ou @.");
    }

    private static bool BeSafeForSpreadsheetExport(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        return value.TrimStart()[0] is not ('=' or '+' or '-' or '@');
    }
}
