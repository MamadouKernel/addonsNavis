using EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;

namespace EscaleReport.Web.Application.Common.Interfaces;

// Application ne connaît pas QuestPDF (ni aucune bibliothèque de rendu) — seule
// Infrastructure implémente cette interface. CDC §14.3 "Export PDF".
public interface IEscalePdfReportGenerator
{
    byte[] Generate(EscaleDetailDto detail, string generatedBy, DateTime generatedAtUtc);
}
