using MediatR;

namespace EscaleReport.Web.Application.Notifications.Queries.GetActiveAlerts;

public record GetActiveAlertsQuery : IRequest<ActiveAlertsResultDto>;
