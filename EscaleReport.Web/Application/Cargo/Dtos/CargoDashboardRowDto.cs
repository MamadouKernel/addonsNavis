namespace EscaleReport.Web.Application.Cargo.Dtos;

public class CargoDashboardRowDto
{
    public Guid EscaleId { get; set; }
    public string Navire { get; set; } = string.Empty;
    public string? Voyage { get; set; }
    public bool DischRealisee { get; set; }
    public bool LoadRealisee { get; set; }
    public bool RevisedLoadRecu { get; set; }

    public int NombreAlertes =>
        (DischRealisee ? 0 : 1) + (LoadRealisee ? 0 : 1) + (RevisedLoadRecu ? 0 : 1);
}
