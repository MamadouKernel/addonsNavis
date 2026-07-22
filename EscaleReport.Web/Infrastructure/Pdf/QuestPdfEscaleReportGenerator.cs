using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;
using EscaleReport.Web.Application.VesselPlanning.Dtos;
using EscaleReport.Web.Domain.VesselPlanning;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EscaleReport.Web.Infrastructure.Pdf;

// CDC §14.3 "Export PDF" : logo/nom du rapport, infos escale, tableaux de synthèse,
// date de génération, auteur, pagination, pied de page, niveau de confidentialité.
// La génération est faite directement par l'application (QuestPDF), pas par la
// fonction d'impression du navigateur, comme exigé par le CDC.
public class QuestPdfEscaleReportGenerator : IEscalePdfReportGenerator
{
    public byte[] Generate(EscaleDetailDto detail, string generatedBy, DateTime generatedAtUtc)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(32);
                page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken3));

                page.Header().Element(c => ComposeHeader(c, detail, generatedBy, generatedAtUtc));
                page.Content().PaddingTop(16).Element(c => ComposeContent(c, detail));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, EscaleDetailDto detail, string generatedBy, DateTime generatedAtUtc)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("ESCALEREPORT").FontSize(16).Bold().FontColor(Colors.Indigo.Darken2);
                col.Item().Text("Côte d'Ivoire Terminal").FontSize(8).FontColor(Colors.Grey.Medium);
                col.Item().PaddingTop(8).Text("Rapport d'escale").FontSize(13).SemiBold();
            });

            row.ConstantItem(180).Column(col =>
            {
                col.Item().AlignRight().Text(detail.Escale.Navire).FontSize(12).Bold();
                col.Item().AlignRight().Text($"Généré le {generatedAtUtc:dd/MM/yyyy HH:mm} UTC").FontSize(8);
                col.Item().AlignRight().Text($"Par {generatedBy}").FontSize(8);
            });
        });
    }

    private static void ComposeContent(IContainer container, EscaleDetailDto detail)
    {
        container.Column(col =>
        {
            col.Item().Background(Colors.Grey.Lighten4).Padding(10).Row(row =>
            {
                InfoCell(row, "Voyage", detail.Escale.Voyage);
                InfoCell(row, "Ligne maritime", detail.Escale.LigneMaritime);
                InfoCell(row, "Quai", detail.Escale.Quai ?? "—");
                InfoCell(row, "ETA", detail.Escale.Eta == default ? "—" : detail.Escale.Eta.ToString("dd/MM/yyyy HH:mm"));
            });

            col.Item().PaddingTop(20).Text("Conteneurs en anomalie").FontSize(11).Bold();

            if (detail.Anomalies.Count == 0)
            {
                col.Item().PaddingTop(6).Text("Aucune anomalie déclarée.").Italic().FontColor(Colors.Grey.Medium);
            }
            else
            {
                col.Item().PaddingTop(6).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        HeaderCell(header, "Conteneur");
                        HeaderCell(header, "Sens");
                        HeaderCell(header, "Ligne");
                        HeaderCell(header, "Position");
                        HeaderCell(header, "Raison");
                        HeaderCell(header, "Statut");
                    });

                    foreach (var a in detail.Anomalies)
                    {
                        BodyCell(table, a.NumeroConteneur);
                        BodyCell(table, a.Sens == Sens.Debarquement ? "Débarquement" : "Embarquement");
                        BodyCell(table, a.LigneMaritime ?? "—");
                        BodyCell(table, a.Position ?? "—");
                        BodyCell(table, a.Raison);
                        BodyCell(table, a.Statut == AnomalyStatus.Resolu ? "Résolu" : "Non résolu");
                    }
                });
            }
        });
    }

    private static void InfoCell(RowDescriptor row, string label, string value)
    {
        row.RelativeItem().Column(col =>
        {
            col.Item().Text(label.ToUpperInvariant()).FontSize(7).FontColor(Colors.Grey.Medium);
            col.Item().Text(value).FontSize(10).SemiBold();
        });
    }

    private static void HeaderCell(TableCellDescriptor header, string text) =>
        header.Cell().Background(Colors.Grey.Darken4).Padding(5)
            .Text(text.ToUpperInvariant()).FontSize(7).FontColor(Colors.White).Bold();

    private static void BodyCell(TableDescriptor table, string text) =>
        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(text).FontSize(9);

    private static void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Text("Confidentiel — Usage interne").FontSize(7).FontColor(Colors.Grey.Medium);
            row.RelativeItem().AlignRight().Text(text =>
            {
                text.CurrentPageNumber().FontSize(7);
                text.Span(" / ").FontSize(7);
                text.TotalPages().FontSize(7);
            });
        });
    }
}
