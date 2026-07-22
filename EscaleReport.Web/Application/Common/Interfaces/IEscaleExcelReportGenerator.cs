using EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;

namespace EscaleReport.Web.Application.Common.Interfaces;

// Application ne connaît pas ClosedXML — seule Infrastructure implémente cette interface.
// CDC §14.4 "Export Excel" : fichier structuré et exploitable pour les analyses.
public interface IEscaleExcelReportGenerator
{
    byte[] Generate(EscaleDetailDto detail);
}
