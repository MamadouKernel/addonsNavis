using ClosedXML.Excel;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;
using EscaleReport.Web.Domain.VesselPlanning;

namespace EscaleReport.Web.Infrastructure.Excel;

// CDC §14.4 "Export Excel" : fichier structuré et exploitable pour les analyses —
// une feuille d'entête escale, une feuille par jeu de données (ici anomalies conteneurs).
public class ClosedXmlEscaleExcelReportGenerator : IEscaleExcelReportGenerator
{
    public byte[] Generate(EscaleDetailDto detail)
    {
        using var workbook = new XLWorkbook();

        var infoSheet = workbook.Worksheets.Add("Escale");
        WriteEscaleInfo(infoSheet, detail);

        var anomaliesSheet = workbook.Worksheets.Add("Anomalies conteneurs");
        WriteAnomalies(anomaliesSheet, detail);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static void WriteEscaleInfo(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        var rows = new (string Label, string Value)[]
        {
            ("Navire", detail.Escale.Navire),
            ("Voyage", detail.Escale.Voyage),
            ("Ligne maritime", detail.Escale.LigneMaritime),
            ("Vessel Visit", detail.Escale.VesselVisit ?? ""),
            ("Quai", detail.Escale.Quai ?? ""),
            ("ETA", detail.Escale.Eta == default ? "" : detail.Escale.Eta.ToString("dd/MM/yyyy HH:mm")),
            ("ATA", detail.Escale.Ata?.ToString("dd/MM/yyyy HH:mm") ?? ""),
            ("Statut opérations", detail.Escale.StatutOperations.ToString()),
            ("Statut planification", detail.Escale.StatutPlanification.ToString())
        };

        for (var i = 0; i < rows.Length; i++)
        {
            var r = i + 1;
            sheet.Cell(r, 1).Value = rows[i].Label;
            sheet.Cell(r, 1).Style.Font.Bold = true;
            sheet.Cell(r, 2).Value = rows[i].Value;
        }

        sheet.Columns(1, 2).AdjustToContents();
    }

    private static void WriteAnomalies(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        string[] headers = ["Conteneur", "Sens", "Ligne maritime", "Position", "Raison", "Statut", "Résolu par", "Date résolution", "Commentaire"];
        for (var c = 0; c < headers.Length; c++)
        {
            var cell = sheet.Cell(1, c + 1);
            cell.Value = headers[c];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(0x1E, 0x29, 0x3B);
            cell.Style.Font.FontColor = XLColor.White;
        }

        var row = 2;
        foreach (var a in detail.Anomalies)
        {
            sheet.Cell(row, 1).Value = a.NumeroConteneur;
            sheet.Cell(row, 2).Value = a.Sens == Sens.Debarquement ? "Débarquement" : "Embarquement";
            sheet.Cell(row, 3).Value = a.LigneMaritime ?? "";
            sheet.Cell(row, 4).Value = a.Position ?? "";
            sheet.Cell(row, 5).Value = a.Raison;
            sheet.Cell(row, 6).Value = a.Statut == AnomalyStatus.Resolu ? "Résolu" : "Non résolu";
            sheet.Cell(row, 7).Value = a.ResoluPar ?? "";
            sheet.Cell(row, 8).Value = a.DateResolutionUtc?.ToString("dd/MM/yyyy HH:mm") ?? "";
            sheet.Cell(row, 9).Value = a.Commentaire ?? "";
            row++;
        }

        sheet.SheetView.FreezeRows(1);
        sheet.RangeUsed()?.SetAutoFilter();
        sheet.Columns().AdjustToContents();
    }
}
