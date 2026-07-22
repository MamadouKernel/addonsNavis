using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Itt;
using MediatR;

namespace EscaleReport.Web.Application.Itt.Commands.AddIttEnginPanne;

public class AddIttEnginPanneCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddIttEnginPanneCommand, Guid>
{
    public async Task<Guid> Handle(AddIttEnginPanneCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var panne = new IttEnginPanne
        {
            Engin = request.Engin,
            DateDebutUtc = request.DateDebutUtc,
            Cause = request.Cause,
            RetireEffectif = request.RetireEffectif
        };

        dbContext.IttEnginPannes.Add(panne);
        await dbContext.SaveChangesAsync(cancellationToken);

        return panne.Id;
    }
}
