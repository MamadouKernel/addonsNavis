using MediatR;

namespace EscaleReport.Web.Application.Settings.Commands.AddGantry;

public record AddGantryCommand(string Code) : IRequest;
