using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.VesselPlanning;
using MediatR;

namespace EscaleReport.Web.Application.VesselPlanning.Commands.AddContainerAnomaly;

public class AddContainerAnomalyCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddContainerAnomalyCommand, Guid>
{
    public async Task<Guid> Handle(AddContainerAnomalyCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var anomaly = new ContainerAnomaly
        {
            EscaleId = request.EscaleId,
            NumeroConteneur = request.NumeroConteneur,
            Sens = request.Sens,
            LigneMaritime = request.LigneMaritime,
            Position = request.Position,
            Raison = request.Raison,
            Commentaire = request.Commentaire,
            ReferenceEchange = request.ReferenceEchange
        };

        dbContext.ContainerAnomalies.Add(anomaly);
        await dbContext.SaveChangesAsync(cancellationToken);

        return anomaly.Id;
    }
}
