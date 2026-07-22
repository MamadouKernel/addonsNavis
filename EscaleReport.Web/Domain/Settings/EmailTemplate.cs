using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Settings;

public static class EmailTemplateKeys
{
    public const string RapportEscale = "RapportEscale";
    public const string RapportShift = "RapportShift";
    public const string ConsommationDisch = "ConsommationDisch";
    public const string ConsommationLoad = "ConsommationLoad";

    public static readonly IReadOnlyDictionary<string, string> Labels = new Dictionary<string, string>
    {
        [RapportEscale] = "Rapport de fin d'escale",
        [RapportShift] = "Rapport de fin de shift",
        [ConsommationDisch] = "Consommation Disch",
        [ConsommationLoad] = "Consommation Load"
    };
}

// CDC §14.5/§15.1 "Modèles d'e-mails" — Sujet/Corps supportent des variables substituées à
// l'envoi ({navire}, {voyage}, {ligne}, {visite} pour les modèles Cargo ; {date}, {shift},
// {navire} pour le rapport de shift), Cle identifie le modèle (voir EmailTemplateKeys).
public class EmailTemplate : BaseAuditableEntity
{
    public string Cle { get; set; } = string.Empty;
    public string Sujet { get; set; } = string.Empty;
    public string Corps { get; set; } = string.Empty;
    public string? DestinatairesParDefaut { get; set; }
}
