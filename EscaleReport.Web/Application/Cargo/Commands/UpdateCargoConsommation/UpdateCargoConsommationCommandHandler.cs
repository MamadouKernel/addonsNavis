using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Cargo;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Cargo.Commands.UpdateCargoConsommation;

public class UpdateCargoConsommationCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateCargoConsommationCommand>
{
    public async Task Handle(UpdateCargoConsommationCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var consommation = await dbContext.CargoConsommations
            .FirstOrDefaultAsync(c => c.EscaleId == request.EscaleId, cancellationToken);

        if (consommation is null)
        {
            consommation = new CargoConsommation { EscaleId = request.EscaleId };
            dbContext.CargoConsommations.Add(consommation);
        }

        consommation.DischHazard = request.DischHazard;
        consommation.DischReefer = request.DischReefer;
        consommation.DischOog = request.DischOog;
        consommation.DischImport = request.DischImport;
        consommation.DischRestow = request.DischRestow;
        consommation.DischTranshipment = request.DischTranshipment;
        consommation.Disch20Pieds = request.Disch20Pieds;
        consommation.Disch40Pieds = request.Disch40Pieds;
        consommation.DischRealisee = request.DischRealisee;

        consommation.LoadYard = request.LoadYard;
        consommation.LoadEnCommunication = request.LoadEnCommunication;
        consommation.LoadRealisee = request.LoadRealisee;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
