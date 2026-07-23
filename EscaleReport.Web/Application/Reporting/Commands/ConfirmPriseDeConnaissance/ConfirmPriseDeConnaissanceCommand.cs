using MediatR;

namespace EscaleReport.Web.Application.Reporting.Commands.ConfirmPriseDeConnaissance;

public record ConfirmPriseDeConnaissanceCommand(DateOnly Date, string? Shift) : IRequest;
