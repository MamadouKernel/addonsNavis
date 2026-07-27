namespace EscaleReport.Web.Models;
public sealed class RecycleBinViewModel
{
    public IReadOnlyList<DeletedEscaleItem> Escales { get; init; } = [];
}
public sealed record DeletedEscaleItem(Guid Id, string Navire, string Voyage, DateTime? DeletedAtUtc, string? DeletedBy);
