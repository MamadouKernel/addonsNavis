using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Dispatch.Queries.GetDispatchAutresEngins;

public class GetDispatchAutresEnginsQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetDispatchAutresEnginsQuery, DispatchAutresEnginsDto>
{
    public async Task<DispatchAutresEnginsDto> Handle(GetDispatchAutresEnginsQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var effectif = await dbContext.AutresEnginsEffectifs.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        var retiresParCategorie = await dbContext.EnginProblemes
            .AsNoTracking()
            .Where(p => p.RetireEffectif && p.DateFinUtc == null)
            .GroupBy(p => p.Categorie)
            .Select(g => new { Categorie = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        int RetirePour(CategorieEngin categorie) =>
            retiresParCategorie.FirstOrDefault(x => x.Categorie == categorie)?.Count ?? 0;

        var problemes = await dbContext.EnginProblemes
            .AsNoTracking()
            .OrderByDescending(p => p.DateDebutUtc)
            .Select(p => new EnginProblemeDto
            {
                Id = p.Id,
                Engin = p.Engin,
                Categorie = p.Categorie,
                DateDebutUtc = p.DateDebutUtc,
                DateFinUtc = p.DateFinUtc,
                Duree = p.Duree,
                Probleme = p.Probleme,
                RetireEffectif = p.RetireEffectif,
                ActionRealisee = p.ActionRealisee,
                EstResolu = p.EstResolu
            }).ToListAsync(cancellationToken);

        var deconnexions = await dbContext.EnginDeconnexions
            .AsNoTracking()
            .OrderByDescending(d => d.DateDebutUtc)
            .Select(d => new EnginDeconnexionDto
            {
                Id = d.Id,
                Engin = d.Engin,
                DateDebutUtc = d.DateDebutUtc,
                DateRetourUtc = d.DateRetourUtc,
                Duree = d.Duree,
                Motif = d.Motif,
                EstResolu = d.EstResolu
            }).ToListAsync(cancellationToken);

        var remplacements = await dbContext.RemplacementsOperateur
            .AsNoTracking()
            .OrderByDescending(r => r.DateHeureUtc)
            .Select(r => new RemplacementOperateurDto
            {
                Id = r.Id,
                Operateur = r.Operateur,
                EnginQuitte = r.EnginQuitte,
                NouvelEngin = r.NouvelEngin,
                DateHeureUtc = r.DateHeureUtc,
                Raison = r.Raison,
                Commentaire = r.Commentaire
            }).ToListAsync(cancellationToken);

        return new DispatchAutresEnginsDto
        {
            Effectif = new AutresEnginsEffectifDto
            {
                DisponibleReachStackers = effectif?.DisponibleReachStackers ?? 0,
                DisponibleEmptyHandlers = effectif?.DisponibleEmptyHandlers ?? 0,
                DisponibleAutres = effectif?.DisponibleAutres ?? 0,
                RetireReachStackers = RetirePour(CategorieEngin.ReachStacker),
                RetireEmptyHandlers = RetirePour(CategorieEngin.EmptyHandler),
                RetireAutres = RetirePour(CategorieEngin.Autre)
            },
            Problemes = problemes,
            Deconnexions = deconnexions,
            Remplacements = remplacements
        };
    }
}
