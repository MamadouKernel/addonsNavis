using MediatR;

namespace EscaleReport.Web.Application.Reporting.Commands.UpsertShiftHandoverNote;

public record UpsertShiftHandoverNoteCommand(
    DateOnly Date,
    string? Shift,
    string? ActionsEnCours,
    string? PointsATransmettre) : IRequest;
