using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Reporting.Queries.GetShiftReport;
using EscaleReport.Web.Domain.Escales;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EscaleReport.Web.Infrastructure.Pdf;

// CDC §14.1 "Rapport de fin de shift" : consolidation multi-postes, générable pour le navire
// courant ou l'ensemble des navires en cours dans un document unique, avec la rubrique
// Planification affichée une seule fois en fin de document.
public class QuestPdfShiftReportGenerator : IShiftReportPdfGenerator
{
    public byte[] Generate(ShiftReportDto report, string generatedBy, DateTime generatedAtUtc)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(32);
                page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken3));

                page.Header().Element(c => ComposeHeader(c, report, generatedBy, generatedAtUtc));
                page.Content().PaddingTop(16).Element(c => ComposeContent(c, report));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, ShiftReportDto report, string generatedBy, DateTime generatedAtUtc)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("ESCALEREPORT").FontSize(16).Bold().FontColor(Colors.Indigo.Darken2);
                col.Item().Text("Côte d'Ivoire Terminal").FontSize(8).FontColor(Colors.Grey.Medium);
                col.Item().PaddingTop(8).Text("Rapport de fin de shift").FontSize(13).SemiBold();
            });

            row.ConstantItem(200).Column(col =>
            {
                col.Item().AlignRight().Text($"{report.Date:dd/MM/yyyy} — {report.Shift ?? "Tous shifts"}").FontSize(12).Bold();
                col.Item().AlignRight().Text($"Coordinateur : {report.Coordinateur}").FontSize(8);
                col.Item().AlignRight().Text($"Généré le {generatedAtUtc:dd/MM/yyyy HH:mm} UTC par {generatedBy}").FontSize(8);
            });
        });
    }

    private static void ComposeContent(IContainer container, ShiftReportDto report)
    {
        container.Column(col =>
        {
            SectionTitle(col, report.NavireCourantMode ? "Navire" : "Navires présents ou attendus");
            if (report.NaviresPresentsOuAttendus.Count == 0)
            {
                EmptyNotice(col, "Aucun navire présent ou attendu pour cette sélection.");
            }
            else
            {
                Table(col, ["Navire", "Voyage", "Quai", "ETA", "État des opérations"], report.NaviresPresentsOuAttendus.Select(n => new[]
                {
                    n.Navire,
                    n.Voyage,
                    n.Quai ?? "—",
                    n.Eta == default ? "—" : n.Eta.ToString("dd/MM HH:mm"),
                    StatutLabel(n.StatutOperations)
                }));
            }

            SectionTitle(col, "Ressources STS");
            col.Item().PaddingTop(6).Row(row =>
            {
                InfoCell(row, "Disponibles", report.SyntheseSts.PortiquesDisponibles.ToString());
                InfoCell(row, "Affectés", report.SyntheseSts.PortiquesAffectes.ToString());
                InfoCell(row, "En panne", report.SyntheseSts.PortiquesEnPanne.ToString());
                InfoCell(row, "Incidents en cours", report.SyntheseSts.IncidentsEnCours.ToString());
            });

            SectionTitle(col, "Ressources TT");
            col.Item().PaddingTop(6).Row(row =>
            {
                InfoCell(row, "Total", report.SyntheseTt.EffectifTotal.ToString());
                InfoCell(row, "Désigné", report.SyntheseTt.EffectifDesigne.ToString());
                InfoCell(row, "Disponible", report.SyntheseTt.EffectifDisponible.ToString());
                InfoCell(row, "Déconnexions", report.SyntheseTt.Deconnexions.ToString());
                InfoCell(row, "Écarts", report.SyntheseTt.EcartsAffectation.ToString());
            });

            SectionTitle(col, "Ressources RTG et autres engins");
            col.Item().PaddingTop(6).Row(row =>
            {
                InfoCell(row, "RTG disponibles", report.SyntheseRtgAutresEngins.RtgDisponible.ToString());
                InfoCell(row, "RTG en panne", report.SyntheseRtgAutresEngins.RtgEnPanne.ToString());
                InfoCell(row, "Autres engins dispo.", report.SyntheseRtgAutresEngins.EnginsDisponibles.ToString());
                InfoCell(row, "Clashs", report.SyntheseRtgAutresEngins.Clashs.ToString());
                InfoCell(row, "Problèmes Gate", report.SyntheseRtgAutresEngins.ProblemesGate.ToString());
            });

            SectionTitle(col, "Pannes et indisponibilités");
            if (report.PannesEtIndisponibilites.Count == 0)
            {
                EmptyNotice(col, "Aucune panne ou indisponibilité en cours.");
            }
            else
            {
                foreach (var p in report.PannesEtIndisponibilites)
                {
                    col.Item().PaddingTop(3).Text($"• {p}").FontSize(9);
                }
            }

            SectionTitle(col, "Incidents");
            if (report.Incidents.Count == 0)
            {
                EmptyNotice(col, "Aucun incident en cours.");
            }
            else
            {
                foreach (var i in report.Incidents)
                {
                    col.Item().PaddingTop(3).Text($"• {i}").FontSize(9);
                }
            }

            SectionTitle(col, "Données Cargo");
            if (report.SyntheseCargo.Count == 0)
            {
                EmptyNotice(col, "Aucune consolidation Cargo enregistrée.");
            }
            else
            {
                Table(col, ["Navire", "Disch fait", "Load fait", "Revised reçu", "Rapport envoyé", "Alertes"], report.SyntheseCargo.Select(c => new[]
                {
                    c.Navire,
                    c.DischFait ? "Oui" : "Non",
                    c.LoadFait ? "Oui" : "Non",
                    c.RevisedLoadRecu ? "Oui" : "Non",
                    c.RapportEnvoye ? "Oui" : "Non",
                    c.AlertesOuvertes.ToString()
                }));
            }

            SectionTitle(col, "Données Yard");
            col.Item().PaddingTop(6).Row(row =>
            {
                InfoCell(row, "Zones débarquement", report.SyntheseYard.ZonesDebarquement.ToString());
                InfoCell(row, "Transferts Out en cours", report.SyntheseYard.TransfertsOutEnCours.ToString());
                InfoCell(row, "Housekeeping", report.SyntheseYard.HousekeepingTotal.ToString());
                InfoCell(row, "Tâches en retard", report.SyntheseYard.TachesEnRetard.ToString());
            });

            SectionTitle(col, "Transferts ITT");
            col.Item().PaddingTop(6).Row(row =>
            {
                InfoCell(row, "Transferts en cours", report.SyntheseItt.TransfertsEnCours.ToString());
                InfoCell(row, "Incidents en cours", report.SyntheseItt.IncidentsEnCours.ToString());
                InfoCell(row, "Équip. disponibles", report.SyntheseItt.EquipementsDisponibles.ToString());
                InfoCell(row, "Équip. en panne", report.SyntheseItt.EquipementsEnPanne.ToString());
            });

            SectionTitle(col, "Actions en cours");
            col.Item().PaddingTop(6).Text(string.IsNullOrWhiteSpace(report.ActionsEnCours) ? "Aucune action en cours renseignée." : report.ActionsEnCours)
                .FontSize(9).Italic(string.IsNullOrWhiteSpace(report.ActionsEnCours));

            SectionTitle(col, "Points à transmettre au shift suivant");
            col.Item().PaddingTop(6).Text(string.IsNullOrWhiteSpace(report.PointsATransmettre) ? "Aucun point à transmettre renseigné." : report.PointsATransmettre)
                .FontSize(9).Italic(string.IsNullOrWhiteSpace(report.PointsATransmettre));

            // CDC : rubrique Planification affichée une seule fois, en fin de document.
            SectionTitle(col, "Planification");
            if (report.Planification.Count == 0)
            {
                EmptyNotice(col, "Aucun navire attendu.");
            }
            else
            {
                Table(col, ["Navire", "Voyage", "Quai", "ETA", "Planification", "Consigne pour la relève"], report.Planification.Select(p => new[]
                {
                    p.Navire,
                    p.Voyage,
                    p.Quai ?? "—",
                    p.Eta == default ? "—" : p.Eta.ToString("dd/MM HH:mm"),
                    PlanifLabel(p.StatutPlanification),
                    p.Commentaire ?? "—"
                }));
            }
        });
    }

    private static string StatutLabel(StatutOperations s) => s switch
    {
        StatutOperations.EnCours => "En cours",
        StatutOperations.Terminees => "Terminées",
        _ => "Pas encore débutées"
    };

    private static string PlanifLabel(StatutPlanification s) => s switch
    {
        StatutPlanification.Planifie => "Planifié",
        StatutPlanification.PlanValide => "Plan validé",
        _ => "Non planifié"
    };

    private static void SectionTitle(ColumnDescriptor col, string title) =>
        col.Item().PaddingTop(20).Text(title).FontSize(11).Bold();

    private static void EmptyNotice(ColumnDescriptor col, string text) =>
        col.Item().PaddingTop(6).Text(text).Italic().FontColor(Colors.Grey.Medium);

    private static void Table(ColumnDescriptor col, string[] headers, IEnumerable<string[]> rows)
    {
        col.Item().PaddingTop(6).Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                foreach (var _ in headers)
                {
                    columns.RelativeColumn(2);
                }
            });

            table.Header(header =>
            {
                foreach (var h in headers)
                {
                    HeaderCell(header, h);
                }
            });

            foreach (var row in rows)
            {
                foreach (var cell in row)
                {
                    BodyCell(table, cell);
                }
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
