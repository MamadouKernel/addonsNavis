using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.YardPlanning;
using MediatR;

namespace EscaleReport.Web.Application.YardPlanning.Commands.AddHousekeepingTask;

public class AddHousekeepingTaskCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddHousekeepingTaskCommand, Guid>
{
    public async Task<Guid> Handle(AddHousekeepingTaskCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var task = new HousekeepingTask
        {
            Description = request.Description,
            Zone = request.Zone,
            Priorite = request.Priorite,
            Responsable = string.IsNullOrWhiteSpace(request.Responsable)
                ? currentUser.UserName
                : request.Responsable.Trim(),
            DatePrevue = request.DatePrevue
        };

        dbContext.HousekeepingTasks.Add(task);
        await dbContext.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}
