using MediatR;

namespace EscaleReport.Web.Application.Escales.Queries.GenerateEscaleReport;

public record GenerateEscaleReportQuery(Guid EscaleId) : IRequest<GenerateEscaleReportResult?>;

public record GenerateEscaleReportResult(byte[] PdfBytes, string FileName);
