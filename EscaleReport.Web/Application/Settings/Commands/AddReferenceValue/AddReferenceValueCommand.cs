using MediatR;

namespace EscaleReport.Web.Application.Settings.Commands.AddReferenceValue;

public record AddReferenceValueCommand(string ListKey, string Value) : IRequest;
