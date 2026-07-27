using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Application.Statistics.Queries.GetIndicators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EscaleReport.Web.Controllers;

// Reporting et indicateurs transverses (CDC §17).
[Authorize(Roles = RoleAccessGroups.Statistics)]
public class StatisticsController(ISender mediator) : Controller
{
    public async Task<IActionResult> Index(
        DateOnly? dateDebut, DateOnly? dateFin, string? shift, string? navire,
        string? ligneMaritime, string? quai, string? typeIncident,
        string? equipement, string? utilisateur, string? equipe,
        CancellationToken cancellationToken)
    {
        var dto = await mediator.Send(
            new GetIndicatorsQuery(dateDebut, dateFin, shift, navire, ligneMaritime, quai, typeIncident, equipement, utilisateur, equipe),
            cancellationToken);
        return View(dto);
    }
}
