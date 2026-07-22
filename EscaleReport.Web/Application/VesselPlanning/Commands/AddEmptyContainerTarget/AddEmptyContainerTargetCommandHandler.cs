using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddEmptyContainerTarget;

public class AddEmptyContainerTargetCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddEmptyContainerTargetCommand, Guid>
{
    public async Task<Guid> Handle(AddEmptyContainerTargetCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var target = new Domain.VesselPlanning.EmptyContainerTarget
        {
            EscaleId = request.EscaleId,
            LigneMaritime = request.LigneMaritime,
            TypeConteneur = request.TypeConteneur,
            QuantiteSouhaitee = request.QuantiteSouhaitee
        };

        dbContext.EmptyContainerTargets.Add(target);
        await dbContext.SaveChangesAsync(cancellationToken);

        return target.Id;
    }
}
