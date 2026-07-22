using EscaleReport.Web.Application.Dispatch.Dtos;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchTt;

public class DispatchTtDto
{
    public TtEffectifDto Effectif { get; set; } = new();
    public IReadOnlyList<TtVesselAssignmentDto> Assignments { get; set; } = [];
    public IReadOnlyList<EscaleOptionDto> EscalesDisponibles { get; set; } = [];
    public IReadOnlyList<TtDeconnexionDto> Deconnexions { get; set; } = [];
}

public class TtDeconnexionDto
{
    public Guid Id { get; set; }
    public string NumeroTt { get; set; } = string.Empty;
    public DateTime DateDebutUtc { get; set; }
    public DateTime? DateRetourUtc { get; set; }
    public TimeSpan? Duree { get; set; }
    public string? Raison { get; set; }
    public bool RetireEffectif { get; set; }
    public bool EstResolue { get; set; }
}

public class TtEffectifDto
{
    public int TotalParc { get; set; }
    public int Designes { get; set; }
    public int NonDesignes { get; set; }
    public string? RaisonNonDesignation { get; set; }
    public int Disponibles { get; set; }
    public int Retires { get; set; }
}

public class TtVesselAssignmentDto
{
    public Guid Id { get; set; }
    public string Navire { get; set; } = string.Empty;
    public int NombrePrevu { get; set; }
    public int NombreAffecte { get; set; }
    public int NombreOperationnel { get; set; }
    public int Ecart { get; set; }
    public string? Observations { get; set; }
}
