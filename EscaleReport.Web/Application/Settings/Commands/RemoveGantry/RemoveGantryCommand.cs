using MediatR;

namespace EscaleReport.Web.Application.Settings.Commands.RemoveGantry;

public record RemoveGantryCommand(Guid Id) : IRequest;
