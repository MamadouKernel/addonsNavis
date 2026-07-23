using MediatR;

namespace EscaleReport.Web.Application.Reporting.Commands.ValidateShiftReport;

public record ValidateShiftReportCommand(
    DateOnly Date,
    string? Shift,
    string? CommentaireValidation) : IRequest;
