using MediatR;

namespace EscaleReport.Web.Application.Reporting.Queries.GenerateShiftReport;

public record GenerateShiftReportQuery(DateOnly Date, string? Shift, Guid? EscaleId) : IRequest<GenerateShiftReportResult>;

public record GenerateShiftReportResult(byte[] PdfBytes, string FileName);
