using EscaleReport.Web.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Cargo.Commands.UpdateCargoConsommation;

public class UpdateCargoConsommationCommandValidator : AbstractValidator<UpdateCargoConsommationCommand>
{
    public UpdateCargoConsommationCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.EscaleId)
            .MustAsync((id, ct) => dbContext.Escales.AnyAsync(e => e.Id == id, ct))
            .WithMessage("Escale introuvable.");

        RuleFor(x => x.DischHazard).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DischReefer).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DischOog).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DischImport).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DischRestow).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DischTranshipment).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Disch20Pieds).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Disch40Pieds).GreaterThanOrEqualTo(0);
        RuleFor(x => x.LoadYard).GreaterThanOrEqualTo(0);
        RuleFor(x => x.LoadEnCommunication).GreaterThanOrEqualTo(0);
    }
}
