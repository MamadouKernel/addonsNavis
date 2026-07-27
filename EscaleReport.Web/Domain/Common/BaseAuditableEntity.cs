namespace EscaleReport.Web.Domain.Common;

public abstract class BaseAuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
    public string? UpdatedBy { get; set; }

    // Métadonnées transversales communes à tous les enregistrements métier.
    public string DataSource { get; set; } = "Manual";
    public string LifecycleStatus { get; set; } = "Active";
    public long Version { get; set; } = 1;

    // Suppression logique globale : les données restent récupérables et auditables.
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public string? DeletedBy { get; set; }
    public string? DeletionReason { get; set; }
}
