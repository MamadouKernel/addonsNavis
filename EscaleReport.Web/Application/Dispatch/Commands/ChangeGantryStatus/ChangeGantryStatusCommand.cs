using EscaleReport.Web.Domain.Dispatch;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.ChangeGantryStatus;

public record ChangeGantryStatusCommand(
    Guid GantryId,
    GantryStatus Statut,
    Guid? EscaleId,
    string? TypeIncident,
    string? Cause,
    DateTime? DateDebutUtc) : IRequest;
