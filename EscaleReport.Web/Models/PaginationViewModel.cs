using EscaleReport.Web.Application.Common.Models;

namespace EscaleReport.Web.Models;

// Vue partielle _Pagination.cshtml : un même écran peut contenir plusieurs tableaux paginés
// indépendamment (ex. Yard Planner a 4 onglets), d'où PageParam distinct par tableau
// (ex. "naviresPage", "transfertsPage") plutôt qu'un seul "page" partagé.
public class PaginationViewModel
{
    public int Page { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }
    public string PageParam { get; init; } = "page";

    public static PaginationViewModel For<T>(PagedResult<T> result, string pageParam = "page") => new()
    {
        Page = result.Page,
        TotalPages = result.TotalPages,
        TotalCount = result.TotalCount,
        PageParam = pageParam
    };
}
