using MediatR;

namespace EscaleReport.Web.Application.Cargo.Commands.MarkRevisedLoadReceived;

public record MarkRevisedLoadReceivedCommand(Guid EscaleId, string? Observations) : IRequest;
