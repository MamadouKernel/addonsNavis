using MediatR;

namespace EscaleReport.Web.Application.Cargo.Commands.UpdateCargoConsommation;

public record UpdateCargoConsommationCommand(
    Guid EscaleId,
    int DischHazard,
    int DischReefer,
    int DischOog,
    int DischImport,
    int DischRestow,
    int DischTranshipment,
    int Disch20Pieds,
    int Disch40Pieds,
    bool DischRealisee,
    int LoadYard,
    int LoadEnCommunication,
    bool LoadRealisee) : IRequest;
