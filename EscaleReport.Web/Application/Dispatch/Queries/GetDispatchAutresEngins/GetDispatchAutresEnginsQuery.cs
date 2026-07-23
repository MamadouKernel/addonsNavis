using EscaleReport.Web.Application.Common.Models;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchAutresEngins;

public record GetDispatchAutresEnginsQuery(
    int ProblemesPage = 1,
    int DeconnexionsPage = 1,
    int RemplacementsPage = 1) : IRequest<DispatchAutresEnginsDto>;
