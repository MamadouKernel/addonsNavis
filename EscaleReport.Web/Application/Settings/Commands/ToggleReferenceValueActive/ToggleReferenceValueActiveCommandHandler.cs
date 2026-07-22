using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Settings.Commands.ToggleReferenceValueActive;

public class ToggleReferenceValueActiveCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<ToggleReferenceValueActiveCommand>
{
    public async Task Handle(ToggleReferenceValueActiveCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierParametres))
        {
            throw new ForbiddenAccessException(Permissions.ModifierParametres);
        }

        var value = await dbContext.ReferenceValues.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);
        if (value is null)
        {
            return;
        }

        value.IsActive = !value.IsActive;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
