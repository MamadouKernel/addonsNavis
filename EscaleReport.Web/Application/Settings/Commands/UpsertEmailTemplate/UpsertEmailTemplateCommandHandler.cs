using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Settings.Commands.UpsertEmailTemplate;

public class UpsertEmailTemplateCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<UpsertEmailTemplateCommand>
{
    public async Task Handle(UpsertEmailTemplateCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierParametres))
        {
            throw new ForbiddenAccessException(Permissions.ModifierParametres);
        }

        var template = await dbContext.EmailTemplates.FirstOrDefaultAsync(t => t.Cle == request.Cle, cancellationToken);
        if (template is null)
        {
            template = new EmailTemplate { Cle = request.Cle };
            dbContext.EmailTemplates.Add(template);
        }

        template.Sujet = request.Sujet;
        template.Corps = request.Corps;
        template.DestinatairesParDefaut = request.DestinatairesParDefaut;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
