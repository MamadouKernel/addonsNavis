using EscaleReport.Web.Domain.Cargo;

namespace EscaleReport.Web.Application.Cargo.Dtos;

public class CargoConsommationDto
{
    public int DischHazard { get; set; }
    public int DischReefer { get; set; }
    public int DischOog { get; set; }
    public int DischImport { get; set; }
    public int DischRestow { get; set; }
    public int DischTranshipment { get; set; }
    public int Disch20Pieds { get; set; }
    public int Disch40Pieds { get; set; }
    public bool DischRealisee { get; set; }
    public int DischTotal { get; set; }

    public int LoadYard { get; set; }
    public int LoadEnCommunication { get; set; }
    public bool LoadRealisee { get; set; }
    public int LoadTotal { get; set; }

    public bool RevisedLoadRecu { get; set; }
    public DateTime? RevisedLoadDateUtc { get; set; }
    public string? RevisedLoadPar { get; set; }
    public string? RevisedLoadObservations { get; set; }

    public static CargoConsommationDto FromEntity(CargoConsommation c) => new()
    {
        DischHazard = c.DischHazard,
        DischReefer = c.DischReefer,
        DischOog = c.DischOog,
        DischImport = c.DischImport,
        DischRestow = c.DischRestow,
        DischTranshipment = c.DischTranshipment,
        Disch20Pieds = c.Disch20Pieds,
        Disch40Pieds = c.Disch40Pieds,
        DischRealisee = c.DischRealisee,
        DischTotal = c.DischTotal,
        LoadYard = c.LoadYard,
        LoadEnCommunication = c.LoadEnCommunication,
        LoadRealisee = c.LoadRealisee,
        LoadTotal = c.LoadTotal,
        RevisedLoadRecu = c.RevisedLoadRecu,
        RevisedLoadDateUtc = c.RevisedLoadDateUtc,
        RevisedLoadPar = c.RevisedLoadPar,
        RevisedLoadObservations = c.RevisedLoadObservations
    };
}
