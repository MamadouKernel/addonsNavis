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

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
