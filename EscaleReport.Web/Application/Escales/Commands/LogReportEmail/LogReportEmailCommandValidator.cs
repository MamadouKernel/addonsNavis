using System.Text.RegularExpressions;
using FluentValidation;

namespace EscaleReport.Web.Application.Escales.Commands.LogReportEmail;

public partial class LogReportEmailCommandValidator : AbstractValidator<LogReportEmailCommand>
{
    public LogReportEmailCommandValidator()
    {
        RuleFor(x => x.Recipients)
            .NotEmpty().WithMessage("Au moins un destinataire est requis.")
            .Must(BeValidEmailList).WithMessage("Un ou plusieurs e-mails semblent invalides (séparez-les par des virgules).");
    }

    private static bool BeValidEmailList(string recipients) =>
        recipients.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .All(email => EmailRegex().IsMatch(email));

    // Exclut ?/&/#/=/% en plus des espaces : ces caractères ne servent jamais dans une adresse
    // e-mail réelle ici, mais permettraient d'injecter un paramètre (cc=, bcc=) dans le "mailto:"
    // construit par concaténation (LogReportEmailCommandHandler ne peut pas percent-encoder le
    // "@" du destinataire sans casser certains clients mail — RFC 6068).
    [GeneratedRegex(@"^[^@\s?&#=%]+@[^@\s?&#=%]+\.[^@\s?&#=%]+$")]
    private static partial Regex EmailRegex();
}
