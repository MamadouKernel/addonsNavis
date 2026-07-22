using EscaleReport.Web.Application.Reporting.Queries.GetShiftReport;

namespace EscaleReport.Web.Application.Common.Interfaces;

// CDC §14.1 "Rapport de fin de shift" — Application ne connaît pas QuestPDF, seule
// Infrastructure implémente cette interface.
public interface IShiftReportPdfGenerator
{
    byte[] Generate(ShiftReportDto report, string generatedBy, DateTime generatedAtUtc);
}
