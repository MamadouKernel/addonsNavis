using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddEnginDeconnexion;

public record AddEnginDeconnexionCommand(
    string Engin,
    DateTime DateDebutUtc,
    string? Motif) : IRequest<Guid>;
