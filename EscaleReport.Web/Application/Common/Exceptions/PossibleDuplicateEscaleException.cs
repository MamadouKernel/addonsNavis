namespace EscaleReport.Web.Application.Common.Exceptions;

// CDC §18 "Unicité d'une escale" : "La solution devra avertir l'utilisateur en cas de
// création probable d'un doublon." — un avertissement, pas un blocage : l'utilisateur peut
// confirmer la création malgré tout (voir CreateEscaleCommand.ConfirmerDoublon).
public class PossibleDuplicateEscaleException(Guid existingEscaleId, string existingNavire, string existingVoyage)
    : Exception($"Une escale similaire existe déjà : {existingNavire} / {existingVoyage}.")
{
    public Guid ExistingEscaleId { get; } = existingEscaleId;
    public string ExistingNavire { get; } = existingNavire;
    public string ExistingVoyage { get; } = existingVoyage;
}
