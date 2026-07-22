using MediatR;

namespace EscaleReport.Web.Application.Settings.Commands.UpsertEmailTemplate;

public record UpsertEmailTemplateCommand(
    string Cle,
    string Sujet,
    string Corps,
    string? DestinatairesParDefaut) : IRequest;
