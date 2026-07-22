using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.Settings.Queries.GetParametrage;

public class GetParametrageQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetParametrageQuery, ParametrageDto>
{
    public async Task<ParametrageDto> Handle(GetParametrageQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ModifierParametres))
        {
            throw new ForbiddenAccessException(Permissions.ModifierParametres);
        }

        var settings = await dbContext.GeneralSettings.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        var allValues = await dbContext.ReferenceValues.AsNoTracking()
            .OrderBy(r => r.ListKey).ThenBy(r => r.SortOrder)
            .ToListAsync(cancellationToken);

        var referenceLists = ReferenceListKeys.Labels.Select(kv => new ReferenceListDto
        {
            ListKey = kv.Key,
            Label = kv.Value,
            Values = allValues.Where(v => v.ListKey == kv.Key)
                .Select(v => new ReferenceValueItemDto { Id = v.Id, Value = v.Value, SortOrder = v.SortOrder, IsActive = v.IsActive })
                .ToList()
        }).ToList();

        // Un portique référencé (affectation ou incident STS) ne peut pas être supprimé — la
        // contrainte FK Restrict sur StsIncident.GantryId lèverait sinon une erreur brute.
        var assignedGantryIds = await dbContext.GantryAssignments.AsNoTracking()
            .Select(a => a.GantryId).Distinct().ToListAsync(cancellationToken);
        var referencedByIncidentGantryIds = await dbContext.StsIncidents.AsNoTracking()
            .Where(i => i.GantryId != null)
            .Select(i => i.GantryId!.Value).Distinct().ToListAsync(cancellationToken);
        var nonSupprimables = assignedGantryIds.Concat(referencedByIncidentGantryIds).ToHashSet();

        var gantries = await dbContext.Gantries.AsNoTracking()
            .OrderBy(g => g.Code)
            .Select(g => new GantryDto { Id = g.Id, Code = g.Code, PeutEtreSupprime = !nonSupprimables.Contains(g.Id) })
            .ToListAsync(cancellationToken);

        var templates = await dbContext.EmailTemplates.AsNoTracking().ToListAsync(cancellationToken);
        var emailTemplates = EmailTemplateKeys.Labels.Select(kv =>
        {
            var existing = templates.FirstOrDefault(t => t.Cle == kv.Key);
            return new EmailTemplateDto
            {
                Id = existing?.Id ?? Guid.Empty,
                Cle = kv.Key,
                Label = kv.Value,
                Sujet = existing?.Sujet ?? string.Empty,
                Corps = existing?.Corps ?? string.Empty,
                DestinatairesParDefaut = existing?.DestinatairesParDefaut
            };
        }).ToList();

        var thresholds = await dbContext.AlertThresholds.AsNoTracking()
            .OrderBy(t => t.Libelle)
            .Select(t => new AlertThresholdDto { Id = t.Id, Cle = t.Cle, Libelle = t.Libelle, ValeurHeures = t.ValeurHeures })
            .ToListAsync(cancellationToken);

        return new ParametrageDto
        {
            GeneralSettings = new GeneralSettingsDto
            {
                NomSociete = settings?.NomSociete,
                PlanificateurParDefaut = settings?.PlanificateurParDefaut
            },
            ReferenceLists = referenceLists,
            Gantries = gantries,
            EmailTemplates = emailTemplates,
            AlertThresholds = thresholds
        };
    }
}
