using EscaleReport.Web.Domain.Common;

namespace EscaleReport.Web.Domain.Escales;

// CDC §14.5 : l'application doit conserver la date d'envoi, l'expéditeur, les destinataires,
// l'objet, le type de rapport envoyé et le statut. Le mailto: ne donnant aucune confirmation
// de remise, "l'envoi" tracé ici est l'ouverture du brouillon dans le client de messagerie
// (best-effort), pas une preuve de délivrance — le PDF reste à joindre manuellement par
// l'utilisateur avant l'envoi réel, comme l'exige le CDC.
public class ReportEmailLog : BaseAuditableEntity
{
    public Guid EscaleId { get; set; }
    public string ReportType { get; set; } = "RapportEscale";
    public string SentBy { get; set; } = string.Empty;
    public string Recipients { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime SentAtUtc { get; set; }
}
