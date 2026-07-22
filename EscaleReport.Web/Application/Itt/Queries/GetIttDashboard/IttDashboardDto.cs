namespace EscaleReport.Web.Application.Itt.Queries.GetIttDashboard;

public class IttDashboardDto
{
    public IReadOnlyList<IttTransferDto> Transfers { get; set; } = [];
    public IReadOnlyList<IttTransferIncidentDto> TransferIncidents { get; set; } = [];
    public IttEquipementEffectifDto EquipementEffectif { get; set; } = new();
    public IReadOnlyList<IttEnginPanneDto> EnginPannes { get; set; } = [];
}

public class IttTransferDto
{
    public Guid Id { get; set; }
    public string NavireConnexion { get; set; } = string.Empty;
    public int NombreATransferer { get; set; }
    public int NombreTransfere { get; set; }
    public int NombreRecu { get; set; }
    public int NombreRestant { get; set; }
    public double PourcentageAvancement { get; set; }
    public string? Observations { get; set; }
}

public class IttTransferIncidentDto
{
    public Guid Id { get; set; }
    public string DifficulteOuObjet { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string? Note { get; set; }
    public string? ActionRealisee { get; set; }
    public bool EstResolu { get; set; }
}

public class IttEquipementEffectifDto
{
    public int Disponible { get; set; }
    public int Engage { get; set; }
    public int EnPanne { get; set; }
    public string? Observations { get; set; }
}

public class IttEnginPanneDto
{
    public Guid Id { get; set; }
    public string Engin { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateFinUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string? Cause { get; set; }
    public bool RetireEffectif { get; set; }
    public string? ActionRealisee { get; set; }
    public bool EstResolue { get; set; }
}
