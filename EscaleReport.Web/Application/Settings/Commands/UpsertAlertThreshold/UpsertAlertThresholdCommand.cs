using MediatR;

namespace EscaleReport.Web.Application.Settings.Commands.UpsertAlertThreshold;

public record UpsertAlertThresholdCommand(string Cle, string Libelle, int ValeurHeures) : IRequest;
