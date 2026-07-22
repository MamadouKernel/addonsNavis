using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Settings.Commands.AddGantry;

public class AddGantryCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddGantryCommand>
{
    public async Task Handle(AddGantryCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierParametres))
        {
            throw new ForbiddenAccessException(Permissions.ModifierParametres);
        }

        dbContext.Gantries.Add(new Gantry { Code = request.Code });
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
