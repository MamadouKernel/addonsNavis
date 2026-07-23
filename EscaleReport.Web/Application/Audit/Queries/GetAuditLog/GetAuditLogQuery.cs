using EscaleReport.Web.Application.Common.Models;
using MediatR;

namespace EscaleReport.Web.Application.Audit.Queries.GetAuditLog;

public record GetAuditLogQuery(
    string? UserName = null,
    string? ActionType = null,
    DateOnly? DateDebut = null,
    DateOnly? DateFin = null,
    int Page = 1,
    int PageSize = Paging.DefaultPageSize) : IRequest<AuditLogResultDto>;
