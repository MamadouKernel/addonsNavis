using EscaleReport.Web.Domain.Dispatch;
using MediatR;

namespace EscaleReport.Web.Application.Dispatch.Commands.AddEnginProbleme;

public record AddEnginProblemeCommand(
    string Engin,
    CategorieEngin Categorie,
    DateTime DateDebutUtc,
    string Probleme,
    bool RetireEffectif) : IRequest<Guid>;
