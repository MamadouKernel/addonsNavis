using MediatR;

namespace EscaleReport.Web.Application.Reporting.Queries.GetShiftReport;

// CDC §14.1 : EscaleId null = "l'ensemble des navires en cours" ; renseigné = "le navire courant".
public record GetShiftReportQuery(DateOnly Date, string? Shift, Guid? EscaleId) : IRequest<ShiftReportDto>;
