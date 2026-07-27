using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;
using EscaleReport.Web.Application.VesselPlanning.Dtos;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.VesselPlanning;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EscaleReport.Web.Infrastructure.Pdf;

// CDC §14.2 "Rapport de fin d'escale" + §14.3 "Export PDF" : consolidation complète —
// infos générales, horaires, statut final, les 5 sections Vessel Planning, consommations
// Cargo, ressources STS/TT utilisées, difficultés/actions/points restants ouverts. Généré
// directement par l'application (QuestPDF), pas par la fonction d'impression du navigateur.
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
                col.Item().PaddingTop(8).Text("Rapport de fin d'escale").FontSize(13).SemiBold();
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
            // ---------- Informations générales, horaires, statut final ----------
            col.Item().Background(Colors.Grey.Lighten4).Padding(10).Row(row =>
            {
                InfoCell(row, "Voyage", detail.Escale.Voyage);
                InfoCell(row, "Ligne maritime", detail.Escale.LigneMaritime);
                InfoCell(row, "Quai", detail.Escale.Quai ?? "—");
                InfoCell(row, "Statut final", StatutLabel(detail.Escale.StatutOperations));
            });
            col.Item().PaddingTop(6).Background(Colors.Grey.Lighten4).Padding(10).Row(row =>
            {
                InfoCell(row, "ETA", detail.Escale.Eta == default ? "—" : detail.Escale.Eta.ToString("dd/MM/yyyy HH:mm"));
                InfoCell(row, "ATA", detail.Escale.Ata?.ToString("dd/MM/yyyy HH:mm") ?? "—");
                InfoCell(row, "ETC", detail.Escale.Etc?.ToString("dd/MM/yyyy HH:mm") ?? "—");
                InfoCell(row, "Planification", detail.Escale.StatutPlanification == StatutPlanification.PlanValide
                    ? "Plan validé" : detail.Escale.StatutPlanification == StatutPlanification.Planifie ? "Planifié" : "Non planifié");
            });

            SectionTitle(col, "1. Conteneurs en anomalie");
            if (detail.Anomalies.TotalCount == 0)
            {
                EmptyNotice(col, "Aucune anomalie déclarée.");
            }
            else
            {
                Table(col, ["Conteneur", "Sens", "Ligne", "Position", "Raison", "Statut"], detail.Anomalies.Items.Select(a => new[]
                {
                    a.NumeroConteneur,
                    a.Sens == Sens.Debarquement ? "Débarquement" : "Embarquement",
                    a.LigneMaritime ?? "—",
                    a.Position ?? "—",
                    a.Raison,
                    a.Statut == AnomalyStatus.Resolu ? "Résolu" : "Non résolu"
                }));
            }

            SectionTitle(col, "2. Conteneurs vides");
            if (detail.ConteneursVides.TotalCount == 0)
            {
                EmptyNotice(col, "Aucune cible de conteneurs vides.");
            }
            else
            {
                Table(col, ["Ligne", "Type", "Souhaitée", "Ajoutée", "Planifiée", "Embarquée", "Coupée", "Restante"], detail.ConteneursVides.Items.Select(v => new[]
                {
                    v.LigneMaritime,
                    v.TypeConteneur,
                    v.QuantiteSouhaitee.ToString(),
                    v.QuantiteAjoutee.ToString(),
                    v.QuantitePlanifiee.ToString(),
                    v.QuantiteEmbarquee.ToString(),
                    v.QuantiteCoupee.ToString(),
                    v.QuantiteRestante.ToString()
                }));
                col.Item().PaddingTop(5)
                    .Text($"Totaux — souhaitée {detail.VidesSouhaiteTotal}, ajoutée {detail.VidesAjouteTotal}, planifiée {detail.VidesPlanifieTotal}, embarquée {detail.VidesEmbarqueTotal}, coupée {detail.VidesCoupeTotal}, restante {detail.VidesResteTotal}")
                    .FontSize(8).Bold();
            }

            SectionTitle(col, "3. Incidents opérationnels");
            if (detail.Incidents.TotalCount == 0)
            {
                EmptyNotice(col, "Aucun incident opérationnel déclaré.");
            }
            else
            {
                Table(col, ["Catégorie", "Localisation", "Début", "Fin", "Durée", "Gravité", "Statut", "Description"], detail.Incidents.Items.Select(i => new[]
                {
                    i.Categorie,
                    i.Localisation ?? "—",
                    i.DateDebutUtc.ToString("dd/MM HH:mm"),
                    i.DateFinUtc?.ToString("dd/MM HH:mm") ?? "—",
                    i.Duree.HasValue ? $"{(int)i.Duree.Value.TotalHours}h{i.Duree.Value.Minutes:D2}" : "—",
                    i.Gravite.ToString(),
                    i.Statut == IncidentStatus.Resolu ? "Résolu" : "En cours",
                    i.Description ?? "—"
                }));
            }

            SectionTitle(col, "3.1 Incidents STS");
            if (detail.IncidentsSts.Count == 0)
            {
                EmptyNotice(col, "Aucun incident STS déclaré sur ce navire.");
            }
            else
            {
                Table(col, ["Portique", "Type", "Début", "Statut"], detail.IncidentsSts.Select(i => new[]
                {
                    i.GantryCode ?? "—",
                    i.TypeIncident,
                    i.DateDebutUtc.ToString("dd/MM HH:mm"),
                    i.EstResolu ? "Repris" : "En cours"
                }));
            }

            SectionTitle(col, "4. Conteneurs additionnels");
            if (detail.ConteneursAdditionnels.TotalCount == 0)
            {
                EmptyNotice(col, "Aucun conteneur additionnel déclaré.");
            }
            else
            {
                Table(col, ["Conteneur", "Ligne", "Sens", "Décision"], detail.ConteneursAdditionnels.Items.Select(c => new[]
                {
                    c.NumeroConteneur,
                    c.LigneMaritime ?? "—",
                    c.Sens == Sens.Debarquement ? "Débarquement" : "Embarquement",
                    DecisionLabel(c.Decision)
                }));
            }

            SectionTitle(col, "5. Conteneurs dangereux");
            if (detail.ConteneursDangereux.TotalCount == 0)
            {
                EmptyNotice(col, "Aucun conteneur dangereux déclaré.");
            }
            else
            {
                Table(col, ["Conteneur", "Classe IMO", "Statut BADT", "Statut opérationnel"], detail.ConteneursDangereux.Items.Select(c => new[]
                {
                    c.NumeroConteneur,
                    c.ClasseImo ?? "—",
                    BadtLabel(c.StatutBadt),
                    OperationnelLabel(c.StatutOperationnel)
                }), row => row[2].Contains("renouveler", StringComparison.OrdinalIgnoreCase));
            }

            SectionTitle(col, "Consommations Cargo et volumes finaux");
            if (detail.Cargo is null)
            {
                EmptyNotice(col, "Aucune consolidation Cargo renseignée pour cette escale.");
            }
            else
            {
                col.Item().PaddingTop(6).Row(row =>
                {
                    InfoCell(row, "Disch réalisé", detail.Cargo.DischRealisee ? "Oui" : "Non");
                    InfoCell(row, "Total Disch", detail.Cargo.DischTotal.ToString());
                    InfoCell(row, "Load réalisé", detail.Cargo.LoadRealisee ? "Oui" : "Non");
                    InfoCell(row, "Total Load", detail.Cargo.LoadTotal.ToString());
                    InfoCell(row, "Revised Load reçu", detail.Cargo.RevisedLoadRecu ? "Oui" : "Non");
                });
            }

            SectionTitle(col, "Ressources utilisées — Portiques STS");
            if (detail.RessourcesSts.Count == 0)
            {
                EmptyNotice(col, "Aucun portique affecté à ce navire.");
            }
            else
            {
                Table(col, ["Portique", "Début", "Fin", "Tâche / zone"], detail.RessourcesSts.Select(a => new[]
                {
                    a.GantryCode,
                    a.HeureDebut.ToString("dd/MM HH:mm"),
                    a.HeureFin?.ToString("dd/MM HH:mm") ?? "—",
                    a.TacheOuZone ?? "—"
                }));
            }

            SectionTitle(col, "Ressources utilisées — TT");
            if (detail.RessourcesTt.Count == 0)
            {
                EmptyNotice(col, "Aucune affectation TT pour ce navire.");
            }
            else
            {
                Table(col, ["Prévu", "Affecté", "Opérationnel", "Écart", "Observations"], detail.RessourcesTt.Select(a => new[]
                {
                    a.NombrePrevu.ToString(),
                    a.NombreAffecte.ToString(),
                    a.NombreOperationnel.ToString(),
                    a.Ecart.ToString(),
                    a.Observations ?? "—"
                }));
            }

            ComposeSynthese(col, detail);
        });
    }

    // CDC §14.2 "Difficultés rencontrées", "Actions réalisées", "Points restants ouverts" —
    // synthétisés à partir des données déjà saisies, jamais ressaisis.
    private static void ComposeSynthese(ColumnDescriptor col, EscaleDetailDto detail)
    {
        var difficultes = detail.Incidents.Items.Where(i => i.Statut == IncidentStatus.EnCours).Select(i => $"{i.Categorie} : {i.Description ?? "—"}")
            .Concat(detail.IncidentsSts.Where(i => !i.EstResolu).Select(i => $"Incident STS ({i.GantryCode ?? "—"}) : {i.TypeIncident}"))
            .ToList();

        var actions = detail.Incidents.Items.Where(i => !string.IsNullOrWhiteSpace(i.ActionRealisee)).Select(i => $"{i.Categorie} : {i.ActionRealisee}")
            .Concat(detail.IncidentsSts.Where(i => !string.IsNullOrWhiteSpace(i.ConditionsReprise)).Select(i => $"Incident STS : {i.ConditionsReprise}"))
            .ToList();

        var pointsOuverts = detail.Anomalies.Items.Count(a => a.Statut == AnomalyStatus.NonResolu)
            + detail.ConteneursDangereux.Items.Count(c => c.StatutOperationnel == DangerousContainerStatus.ASuivre)
            + detail.ConteneursAdditionnels.Items.Count(c => c.Decision == AdditionalContainerDecision.EnAttente)
            + detail.Incidents.Items.Count(i => i.Statut == IncidentStatus.EnCours);

        SectionTitle(col, "Difficultés rencontrées");
        if (difficultes.Count == 0)
        {
            EmptyNotice(col, "Aucune difficulté en cours signalée.");
        }
        else
        {
            foreach (var d in difficultes)
            {
                col.Item().PaddingTop(3).Text($"• {d}").FontSize(9);
            }
        }

        SectionTitle(col, "Actions réalisées");
        if (actions.Count == 0)
        {
            EmptyNotice(col, "Aucune action renseignée.");
        }
        else
        {
            foreach (var a in actions)
            {
                col.Item().PaddingTop(3).Text($"• {a}").FontSize(9);
            }
        }

        SectionTitle(col, "Points restants ouverts");
        col.Item().PaddingTop(6).Text($"{pointsOuverts} point(s) restant(s) ouvert(s) : anomalies non résolues, conteneurs dangereux à suivre, conteneurs additionnels en attente et incidents opérationnels en cours.")
            .FontSize(9).FontColor(pointsOuverts > 0 ? Colors.Red.Darken1 : Colors.Green.Darken1);
    }

    private static string StatutLabel(StatutOperations s) => s switch
    {
        StatutOperations.EnCours => "En cours",
        StatutOperations.Terminees => "Terminées",
        _ => "Pas encore débutées"
    };

    private static string DecisionLabel(AdditionalContainerDecision d) => d switch
    {
        AdditionalContainerDecision.Debarque => "Débarqué",
        AdditionalContainerDecision.Embarque => "Embarqué",
        AdditionalContainerDecision.Refuse => "Refusé",
        AdditionalContainerDecision.Reporte => "Reporté",
        _ => "En attente"
    };

    private static string BadtLabel(BadtStatus s) => s switch
    {
        BadtStatus.Pris => "Pris",
        BadtStatus.ARenouveler => "⚠ À renouveler",
        _ => "Non pris"
    };

    private static string OperationnelLabel(DangerousContainerStatus s) => s switch
    {
        DangerousContainerStatus.Cloture => "Clôturé",
        DangerousContainerStatus.Reembarque => "Réembarqué",
        _ => "À suivre"
    };

    private static void SectionTitle(ColumnDescriptor col, string title) =>
        col.Item().PaddingTop(20).Text(title).FontSize(11).Bold();

    private static void EmptyNotice(ColumnDescriptor col, string text) =>
        col.Item().PaddingTop(6).Text(text).Italic().FontColor(Colors.Grey.Medium);

    private static void Table(
        ColumnDescriptor col,
        string[] headers,
        IEnumerable<string[]> rows,
        Func<string[], bool>? highlightRow = null)
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
                var highlighted = highlightRow?.Invoke(row) == true;
                foreach (var cell in row)
                {
                    BodyCell(table, cell, highlighted);
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

    private static void BodyCell(TableDescriptor table, string text, bool highlighted = false) =>
        table.Cell()
            .Background(highlighted ? Colors.Red.Lighten4 : Colors.White)
            .BorderBottom(0.5f)
            .BorderColor(highlighted ? Colors.Red.Lighten2 : Colors.Grey.Lighten2)
            .Padding(5)
            .Text(text)
            .FontSize(9)
            .FontColor(highlighted ? Colors.Red.Darken2 : Colors.Grey.Darken4);

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
