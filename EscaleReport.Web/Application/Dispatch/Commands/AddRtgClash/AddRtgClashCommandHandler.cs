using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Dispatch;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddRtgClash;

public class AddRtgClashCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddRtgClashCommand, Guid>
{
    public async Task<Guid> Handle(AddRtgClashCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule) ||
            !DispatchAccessControl.CanAccessPoste(currentUser, "RTG"))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var clash = new RtgClash
        {
            Lieu = request.Lieu,
            EnginsConcernes = request.EnginsConcernes,
            DateDebutUtc = request.DateDebutUtc,
            Description = request.Description
        };

        dbContext.RtgClashes.Add(clash);
        await dbContext.SaveChangesAsync(cancellationToken);

        return clash.Id;
    }
}
