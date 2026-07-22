using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddRtgClash;

public record AddRtgClashCommand(
    string Lieu,
    string EnginsConcernes,
    DateTime DateDebutUtc,
    string Description) : IRequest<Guid>;
