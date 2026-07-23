using ClosedXML.Excel;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Application.Escales.Queries.GetEscaleDetail;
using EscaleReport.Web.Domain.VesselPlanning;

namespace EscaleReport.Web.Infrastructure.Excel;

// CDC §14.2 "Rapport de fin d'escale" + §14.4 "Export Excel" : une feuille par jeu de
// données pour permettre les analyses, alignée sur le même périmètre que le rapport PDF.
public class ClosedXmlEscaleExcelReportGenerator : IEscaleExcelReportGenerator
{
    public byte[] Generate(EscaleDetailDto detail)
    {
        using var workbook = new XLWorkbook();

        WriteEscaleInfo(workbook.Worksheets.Add("Escale"), detail);
        WriteAnomalies(workbook.Worksheets.Add("Anomalies conteneurs"), detail);
        WriteVides(workbook.Worksheets.Add("Conteneurs vides"), detail);
        WriteIncidents(workbook.Worksheets.Add("Incidents opérationnels"), detail);
        WriteIncidentsSts(workbook.Worksheets.Add("Incidents STS"), detail);
        WriteAdditionnels(workbook.Worksheets.Add("Conteneurs additionnels"), detail);
        WriteDangereux(workbook.Worksheets.Add("Conteneurs dangereux"), detail);
        WriteCargo(workbook.Worksheets.Add("Cargo"), detail);
        WriteRessources(workbook.Worksheets.Add("Ressources STS-TT"), detail);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static void WriteEscaleInfo(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        var rows = new (string Label, string Value)[]
        {
            ("Navire", SafeText(detail.Escale.Navire)),
            ("Voyage", SafeText(detail.Escale.Voyage)),
            ("Ligne maritime", SafeText(detail.Escale.LigneMaritime)),
            ("Vessel Visit", SafeText(detail.Escale.VesselVisit)),
            ("Quai", SafeText(detail.Escale.Quai)),
            ("ETA", detail.Escale.Eta == default ? "" : detail.Escale.Eta.ToString("dd/MM/yyyy HH:mm")),
            ("ATA", detail.Escale.Ata?.ToString("dd/MM/yyyy HH:mm") ?? ""),
            ("ETC", detail.Escale.Etc?.ToString("dd/MM/yyyy HH:mm") ?? ""),
            ("Statut opérations (final)", detail.Escale.StatutOperations.ToString()),
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

    // OWASP "CSV/Excel Formula Injection" : un champ libre (commentaire, observation...) qui
    // commence par ces caractères serait interprété comme une formule par certains tableurs.
    // On préfixe d'une apostrophe pour forcer un affichage en texte, comme le ferait Excel
    // lui-même si l'utilisateur tapait cette valeur directement dans une cellule.
    private static string SafeText(string? value) =>
        string.IsNullOrEmpty(value) || value[0] is not ('=' or '+' or '-' or '@' or '\t' or '\r')
            ? value ?? ""
            : "'" + value;

    private static void WriteHeaders(IXLWorksheet sheet, string[] headers)
    {
        for (var c = 0; c < headers.Length; c++)
        {
            var cell = sheet.Cell(1, c + 1);
            cell.Value = headers[c];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(0x1E, 0x29, 0x3B);
            cell.Style.Font.FontColor = XLColor.White;
        }
    }

    private static void Finalize(IXLWorksheet sheet)
    {
        sheet.SheetView.FreezeRows(1);
        sheet.RangeUsed()?.SetAutoFilter();
        sheet.Columns().AdjustToContents();
    }

    private static void WriteAnomalies(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        WriteHeaders(sheet, ["Conteneur", "Sens", "Ligne maritime", "Position", "Raison", "Statut", "Résolu par", "Date résolution", "Commentaire"]);

        var row = 2;
        foreach (var a in detail.Anomalies)
        {
            sheet.Cell(row, 1).Value = SafeText(a.NumeroConteneur);
            sheet.Cell(row, 2).Value = a.Sens == Sens.Debarquement ? "Débarquement" : "Embarquement";
            sheet.Cell(row, 3).Value = SafeText(a.LigneMaritime);
            sheet.Cell(row, 4).Value = SafeText(a.Position);
            sheet.Cell(row, 5).Value = SafeText(a.Raison);
            sheet.Cell(row, 6).Value = a.Statut == AnomalyStatus.Resolu ? "Résolu" : "Non résolu";
            sheet.Cell(row, 7).Value = SafeText(a.ResoluPar);
            sheet.Cell(row, 8).Value = a.DateResolutionUtc?.ToString("dd/MM/yyyy HH:mm") ?? "";
            sheet.Cell(row, 9).Value = SafeText(a.Commentaire);
            row++;
        }

        Finalize(sheet);
    }

    private static void WriteVides(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        WriteHeaders(sheet, ["Ligne maritime", "Type conteneur", "Souhaitée", "Ajoutée", "Planifiée", "Embarquée", "Coupée", "Motif coupure", "Restante"]);

        var row = 2;
        foreach (var v in detail.ConteneursVides)
        {
            sheet.Cell(row, 1).Value = SafeText(v.LigneMaritime);
            sheet.Cell(row, 2).Value = SafeText(v.TypeConteneur);
            sheet.Cell(row, 3).Value = v.QuantiteSouhaitee;
            sheet.Cell(row, 4).Value = v.QuantiteAjoutee;
            sheet.Cell(row, 5).Value = v.QuantitePlanifiee;
            sheet.Cell(row, 6).Value = v.QuantiteEmbarquee;
            sheet.Cell(row, 7).Value = v.QuantiteCoupee;
            sheet.Cell(row, 8).Value = SafeText(v.MotifCoupure);
            sheet.Cell(row, 9).Value = v.QuantiteRestante;
            row++;
        }

        Finalize(sheet);
    }

    private static void WriteIncidents(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        WriteHeaders(sheet, ["Catégorie", "Localisation", "Début", "Fin", "Durée", "Gravité", "Statut", "Description", "Action réalisée", "Déclaré par"]);

        var row = 2;
        foreach (var i in detail.Incidents)
        {
            sheet.Cell(row, 1).Value = SafeText(i.Categorie);
            sheet.Cell(row, 2).Value = SafeText(i.Localisation);
            sheet.Cell(row, 3).Value = i.DateDebutUtc.ToString("dd/MM/yyyy HH:mm");
            sheet.Cell(row, 4).Value = i.DateFinUtc?.ToString("dd/MM/yyyy HH:mm") ?? "";
            sheet.Cell(row, 5).Value = i.Duree.HasValue ? $"{(int)i.Duree.Value.TotalHours}h{Math.Abs(i.Duree.Value.Minutes):D2}" : "";
            sheet.Cell(row, 6).Value = i.Gravite.ToString();
            sheet.Cell(row, 7).Value = i.Statut == IncidentStatus.Resolu ? "Résolu" : "En cours";
            sheet.Cell(row, 8).Value = SafeText(i.Description);
            sheet.Cell(row, 9).Value = SafeText(i.ActionRealisee);
            sheet.Cell(row, 10).Value = SafeText(i.DeclarePar);
            row++;
        }

        Finalize(sheet);
    }

    private static void WriteIncidentsSts(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        WriteHeaders(sheet, ["Portique", "Type", "Début", "Fin", "Cause", "Retrait effectif", "Statut"]);

        var row = 2;
        foreach (var i in detail.IncidentsSts)
        {
            sheet.Cell(row, 1).Value = SafeText(i.GantryCode);
            sheet.Cell(row, 2).Value = SafeText(i.TypeIncident);
            sheet.Cell(row, 3).Value = i.DateDebutUtc.ToString("dd/MM/yyyy HH:mm");
            sheet.Cell(row, 4).Value = i.DateFinUtc?.ToString("dd/MM/yyyy HH:mm") ?? "";
            sheet.Cell(row, 5).Value = SafeText(i.Cause);
            sheet.Cell(row, 6).Value = i.RetirePortiqueEffectif ? "Oui" : "Non";
            sheet.Cell(row, 7).Value = i.EstResolu ? "Repris" : "En cours";
            row++;
        }

        Finalize(sheet);
    }

    private static void WriteAdditionnels(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        WriteHeaders(sheet, ["Conteneur", "Ligne maritime", "Sens", "Position", "Décision", "Commentaire"]);

        var row = 2;
        foreach (var c in detail.ConteneursAdditionnels)
        {
            sheet.Cell(row, 1).Value = SafeText(c.NumeroConteneur);
            sheet.Cell(row, 2).Value = SafeText(c.LigneMaritime);
            sheet.Cell(row, 3).Value = c.Sens == Sens.Debarquement ? "Débarquement" : "Embarquement";
            sheet.Cell(row, 4).Value = SafeText(c.Position);
            sheet.Cell(row, 5).Value = c.Decision.ToString();
            sheet.Cell(row, 6).Value = SafeText(c.Commentaire);
            row++;
        }

        Finalize(sheet);
    }

    private static void WriteDangereux(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        WriteHeaders(sheet, ["Conteneur", "Ligne maritime", "Classe IMO", "Position", "Statut BADT", "Date validité BADT", "Statut opérationnel", "Commentaire"]);

        var row = 2;
        foreach (var c in detail.ConteneursDangereux)
        {
            sheet.Cell(row, 1).Value = SafeText(c.NumeroConteneur);
            sheet.Cell(row, 2).Value = SafeText(c.LigneMaritime);
            sheet.Cell(row, 3).Value = SafeText(c.ClasseImo);
            sheet.Cell(row, 4).Value = SafeText(c.Position);
            sheet.Cell(row, 5).Value = c.StatutBadt.ToString();
            sheet.Cell(row, 6).Value = c.DateValiditeBadt?.ToString("dd/MM/yyyy") ?? "";
            sheet.Cell(row, 7).Value = c.StatutOperationnel.ToString();
            sheet.Cell(row, 8).Value = SafeText(c.Commentaire);
            row++;
        }

        Finalize(sheet);
    }

    private static void WriteCargo(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        if (detail.Cargo is null)
        {
            sheet.Cell(1, 1).Value = "Aucune consolidation Cargo renseignée pour cette escale.";
            sheet.Column(1).AdjustToContents();
            return;
        }

        var c = detail.Cargo;
        var rows = new (string Label, string Value)[]
        {
            ("Disch — Hazard", c.DischHazard.ToString()),
            ("Disch — Reefer", c.DischReefer.ToString()),
            ("Disch — OOG", c.DischOog.ToString()),
            ("Disch — Import", c.DischImport.ToString()),
            ("Disch — Restow", c.DischRestow.ToString()),
            ("Disch — Transhipment", c.DischTranshipment.ToString()),
            ("Disch — 20 pieds", c.Disch20Pieds.ToString()),
            ("Disch — 40 pieds", c.Disch40Pieds.ToString()),
            ("Disch réalisé", c.DischRealisee ? "Oui" : "Non"),
            ("Total Disch", c.DischTotal.ToString()),
            ("Load — Yard", c.LoadYard.ToString()),
            ("Load — En communication", c.LoadEnCommunication.ToString()),
            ("Load réalisé", c.LoadRealisee ? "Oui" : "Non"),
            ("Total Load", c.LoadTotal.ToString()),
            ("Revised Load reçu", c.RevisedLoadRecu ? "Oui" : "Non"),
            ("Revised Load — date", c.RevisedLoadDateUtc?.ToString("dd/MM/yyyy HH:mm") ?? ""),
            ("Revised Load — par", SafeText(c.RevisedLoadPar)),
            ("Revised Load — observations", SafeText(c.RevisedLoadObservations))
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

    private static void WriteRessources(IXLWorksheet sheet, EscaleDetailDto detail)
    {
        sheet.Cell(1, 1).Value = "Portiques STS";
        sheet.Cell(1, 1).Style.Font.Bold = true;

        sheet.Cell(2, 1).Value = "Portique";
        sheet.Cell(2, 2).Value = "Début";
        sheet.Cell(2, 3).Value = "Fin";
        sheet.Cell(2, 4).Value = "Tâche / zone";
        sheet.Range(2, 1, 2, 4).Style.Font.Bold = true;

        var row = 3;
        foreach (var a in detail.RessourcesSts)
        {
            sheet.Cell(row, 1).Value = SafeText(a.GantryCode);
            sheet.Cell(row, 2).Value = a.HeureDebut.ToString("dd/MM/yyyy HH:mm");
            sheet.Cell(row, 3).Value = a.HeureFin?.ToString("dd/MM/yyyy HH:mm") ?? "";
            sheet.Cell(row, 4).Value = SafeText(a.TacheOuZone);
            row++;
        }

        row += 2;
        sheet.Cell(row, 1).Value = "Terminal Tractors";
        sheet.Cell(row, 1).Style.Font.Bold = true;
        row++;
        sheet.Cell(row, 1).Value = "Prévu";
        sheet.Cell(row, 2).Value = "Affecté";
        sheet.Cell(row, 3).Value = "Opérationnel";
        sheet.Cell(row, 4).Value = "Écart";
        sheet.Cell(row, 5).Value = "Observations";
        sheet.Range(row, 1, row, 5).Style.Font.Bold = true;
        row++;

        foreach (var a in detail.RessourcesTt)
        {
            sheet.Cell(row, 1).Value = a.NombrePrevu;
            sheet.Cell(row, 2).Value = a.NombreAffecte;
            sheet.Cell(row, 3).Value = a.NombreOperationnel;
            sheet.Cell(row, 4).Value = a.Ecart;
            sheet.Cell(row, 5).Value = SafeText(a.Observations);
            row++;
        }

        sheet.Columns().AdjustToContents();
    }
}
