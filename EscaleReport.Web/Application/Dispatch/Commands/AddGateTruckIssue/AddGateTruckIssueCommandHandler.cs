using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddGateTruckIssue;

public class AddGateTruckIssueCommandHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<AddGateTruckIssueCommand, Guid>
{
    public async Task<Guid> Handle(AddGateTruckIssueCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.SaisirDonneesModule))
        {
            throw new ForbiddenAccessException(Permissions.SaisirDonneesModule);
        }

        var issue = new GateTruckIssue
        {
            TypeOperation = request.TypeOperation,
            CamionReference = request.CamionReference,
            DateDebutUtc = request.DateDebutUtc,
            ProblemeRencontre = request.ProblemeRencontre
        };

        dbContext.GateTruckIssues.Add(issue);
        await dbContext.SaveChangesAsync(cancellationToken);

        return issue.Id;
    }
}
