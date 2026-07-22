using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Settings.Commands.AddReferenceValue;

public class AddReferenceValueCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddReferenceValueCommand>
{
    public async Task Handle(AddReferenceValueCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierParametres))
        {
            throw new ForbiddenAccessException(Permissions.ModifierParametres);
        }

        var maxSortOrder = await dbContext.ReferenceValues
            .Where(r => r.ListKey == request.ListKey)
            .Select(r => (int?)r.SortOrder)
            .MaxAsync(cancellationToken) ?? -1;

        dbContext.ReferenceValues.Add(new ReferenceValue
        {
            ListKey = request.ListKey,
            Value = request.Value,
            SortOrder = maxSortOrder + 1
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
