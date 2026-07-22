using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Settings.Commands.UpdateGeneralSettings;

public class UpdateGeneralSettingsCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpdateGeneralSettingsCommand>
{
    public async Task Handle(UpdateGeneralSettingsCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierParametres))
        {
            throw new ForbiddenAccessException(Permissions.ModifierParametres);
        }

        var settings = await dbContext.GeneralSettings.FirstOrDefaultAsync(cancellationToken);
        if (settings is null)
        {
            settings = new Domain.Settings.GeneralSettings();
            dbContext.GeneralSettings.Add(settings);
        }

        settings.NomSociete = request.NomSociete;
        settings.PlanificateurParDefaut = request.PlanificateurParDefaut;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
