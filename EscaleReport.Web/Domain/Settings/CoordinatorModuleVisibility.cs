using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Settings;

// CDC §12 "Choix des modules affichés" (dépend de §15) : l'Administrateur décide quelles
// synthèses apparaissent sur la vue consolidée du Coordinateur. Une clé absente en base
// (aucune ligne créée pour ce module) est traitée comme visible par défaut — voir
// GetCoordinatorDashboardQueryHandler et GetParametrageQueryHandler — pour ne pas exiger un
// seed explicite des 7 lignes à l'installation.
public static class CoordinatorModuleKeys
{
    public const string Navires = "Navires";
    public const string Sts = "Sts";
    public const string Tt = "Tt";
    public const string RtgAutresEngins = "RtgAutresEngins";
    public const string Cargo = "Cargo";
    public const string Yard = "Yard";
    public const string Itt = "Itt";

    public static readonly IReadOnlyDictionary<string, string> Labels = new Dictionary<string, string>
    {
        [Navires] = "Synthèse navires",
        [Sts] = "Synthèse STS",
        [Tt] = "Synthèse TT",
        [RtgAutresEngins] = "Synthèse RTG et autres engins",
        [Cargo] = "Synthèse Cargo",
        [Yard] = "Synthèse Yard",
        [Itt] = "Synthèse ITT"
    };
}

public class CoordinatorModuleVisibility : BaseAuditableEntity
{
    public string Cle { get; set; } = string.Empty;
    public bool EstVisible { get; set; } = true;
}
