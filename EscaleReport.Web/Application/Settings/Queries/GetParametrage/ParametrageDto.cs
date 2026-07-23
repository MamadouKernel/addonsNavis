namespace EscaleReport.Web.Application.Settings.Queries.GetParametrage;

public class ParametrageDto
{
    public GeneralSettingsDto GeneralSettings { get; set; } = new();
    public IReadOnlyList<ReferenceListDto> ReferenceLists { get; set; } = [];
    public IReadOnlyList<GantryDto> Gantries { get; set; } = [];
    public IReadOnlyList<EmailTemplateDto> EmailTemplates { get; set; } = [];
    public IReadOnlyList<AlertThresholdDto> AlertThresholds { get; set; } = [];
    public IReadOnlyList<CoordinatorModuleVisibilityDto> CoordinatorModules { get; set; } = [];
}

public class GeneralSettingsDto
{
    public string? NomSociete { get; set; }
    public string? PlanificateurParDefaut { get; set; }
}

public class ReferenceListDto
{
    public string ListKey { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public IReadOnlyList<ReferenceValueItemDto> Values { get; set; } = [];
}

public class ReferenceValueItemDto
{
    public Guid Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}

public class GantryDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public bool PeutEtreSupprime { get; set; }
}

public class EmailTemplateDto
{
    public Guid Id { get; set; }
    public string Cle { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Sujet { get; set; } = string.Empty;
    public string Corps { get; set; } = string.Empty;
    public string? DestinatairesParDefaut { get; set; }
}

public class AlertThresholdDto
{
    public Guid Id { get; set; }
    public string Cle { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public int ValeurHeures { get; set; }
}

public class CoordinatorModuleVisibilityDto
{
    public string Cle { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public bool EstVisible { get; set; }
}
