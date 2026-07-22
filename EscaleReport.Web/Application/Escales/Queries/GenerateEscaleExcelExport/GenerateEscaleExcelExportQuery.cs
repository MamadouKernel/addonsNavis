using MediatR;

namespace EscaleReport.Web.Application.Escales.Queries.GenerateEscaleExcelExport;

public record GenerateEscaleExcelExportQuery(Guid EscaleId) : IRequest<GenerateEscaleExcelExportResult?>;

public record GenerateEscaleExcelExportResult(byte[] ExcelBytes, string FileName);
