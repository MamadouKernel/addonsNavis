namespace EscaleReport.Web.Application.Common.Models;

// Résolution partagée d'un couple (page, pageSize) : toujours clampée dans [1, totalPages],
// jamais d'exception ni de page vide sur un numéro de page invalide (0, négatif, ou au-delà
// du nombre réel de pages) — utilisée aussi bien par PagedResult<T>.Create (pagination en
// mémoire) que par les quelques handlers qui paginent directement en base (Skip/Take EF).
public static class Paging
{
    public const int DefaultPageSize = 20;

    public static (int Page, int Skip, int PageSize, int TotalPages) Resolve(int page, int pageSize, int totalCount)
    {
        var safePageSize = pageSize <= 0 ? DefaultPageSize : pageSize;
        var totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)safePageSize);
        var safePage = Math.Clamp(page <= 0 ? 1 : page, 1, totalPages);
        return (safePage, (safePage - 1) * safePageSize, safePageSize, totalPages);
    }
}

// Pagination réutilisable pour tous les tableaux de l'application. La plupart des handlers
// construisent déjà leur liste en mémoire (jointures/calculs C# sur plusieurs requêtes EF) avant
// de la renvoyer : on découpe donc ici, une fois la liste complète assemblée, plutôt que de
// pousser Skip/Take dans chaque requête EF individuellement — plus simple et sans risque de
// régression sur des handlers qui combinent déjà plusieurs sources. Le journal d'audit et les
// tableaux "un enregistrement par escale" (Escales, Cargo), seules tables dont la volumétrie
// peut réellement devenir importante, paginent directement en base via Paging.Resolve ci-dessus.
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = Paging.DefaultPageSize;
    public int TotalCount { get; init; }

    public int TotalPages => TotalCount == 0 ? 1 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    public static PagedResult<T> Create(IReadOnlyList<T> all, int page, int pageSize = Paging.DefaultPageSize)
    {
        var (safePage, skip, safePageSize, _) = Paging.Resolve(page, pageSize, all.Count);

        return new PagedResult<T>
        {
            Items = all.Skip(skip).Take(safePageSize).ToList(),
            Page = safePage,
            PageSize = safePageSize,
            TotalCount = all.Count
        };
    }
}
