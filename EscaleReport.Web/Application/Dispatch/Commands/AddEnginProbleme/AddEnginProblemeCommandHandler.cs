using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddEnginProbleme;

public class AddEnginProblemeCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddEnginProblemeCommand, Guid>
{
    public async Task<Guid> Handle(AddEnginProblemeCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var probleme = new EnginProbleme
        {
            Engin = request.Engin,
            Categorie = request.Categorie,
            DateDebutUtc = request.DateDebutUtc,
            Probleme = request.Probleme,
            RetireEffectif = request.RetireEffectif
        };

        dbContext.EnginProblemes.Add(probleme);
        await dbContext.SaveChangesAsync(cancellationToken);

        return probleme.Id;
    }
}
