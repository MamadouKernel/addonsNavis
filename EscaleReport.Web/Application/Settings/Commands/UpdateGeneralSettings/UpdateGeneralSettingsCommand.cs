using MediatR;

namespace EscaleReport.Web.Application.Settings.Commands.UpdateGeneralSettings;

public record UpdateGeneralSettingsCommand(string? NomSociete, string? PlanificateurParDefaut) : IRequest;
