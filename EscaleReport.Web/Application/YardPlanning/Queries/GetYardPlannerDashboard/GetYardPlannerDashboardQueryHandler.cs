using EscaleReport.Web.Application.Common.Exceptions;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Application.YardPlanning.Queries.GetYardPlannerDashboard;

public class GetYardPlannerDashboardQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser) : IRequestHandler<GetYardPlannerDashboardQuery, YardPlannerDashboardDto>
{
    public async Task<YardPlannerDashboardDto> Handle(GetYardPlannerDashboardQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.HasPermission(Permissions.ConsulterEscales))
        {
            throw new ForbiddenAccessException(Permissions.ConsulterEscales);
        }

        var naviresEnCours = await dbContext.Escales
            .AsNoTracking()
            .OrderBy(e => e.Eta)
            .Select(e => new NavireEnCoursDto
            {
                Id = e.Id,
                Navire = e.Navire,
                Voyage = e.Voyage,
                Quai = e.Quai,
                LigneMaritime = e.LigneMaritime,
                Eta = e.Eta,
                StatutOperations = e.StatutOperations,
                StatutPlanification = e.StatutPlanification
            }).ToListAsync(cancellationToken);

        var plansRaw = await (
            from p in dbContext.VesselYardPlans.AsNoTracking()
            join e in dbContext.Escales.AsNoTracking() on p.EscaleId equals e.Id
            orderby p.CreatedAtUtc descending
            select new { p, e.Navire }).ToListAsync(cancellationToken);

        var vesselYardPlans = plansRaw.Select(x => new VesselYardPlanDto
        {
            Id = x.p.Id,
            Navire = x.Navire,
            ServiceMaritime = x.p.ServiceMaritime,
            ZoneDebarquement = x.p.ZoneDebarquement,
            ReefersImport = x.p.ReefersImport,
            ReefersExport = x.p.ReefersExport,
            ConteneursTransbordement = x.p.ConteneursTransbordement,
            Observations = x.p.Observations
        }).ToList();

        var transfertsOut = await dbContext.TransfertsOut
            .AsNoTracking()
            .OrderByDescending(t => t.HeureDebut)
            .Select(t => new TransfertOutDto
            {
                Id = t.Id,
                Bay = t.Bay,
                NombreConteneurs = t.NombreConteneurs,
                DetailOuDestination = t.DetailOuDestination,
                HeureDebut = t.HeureDebut,
                HeureFin = t.HeureFin,
                Commentaire = t.Commentaire,
                EstTermine = t.EstTermine
            }).ToListAsync(cancellationToken);

        var housekeepingTasks = await dbContext.HousekeepingTasks
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAtUtc)
            .Select(t => new HousekeepingTaskDto
            {
                Id = t.Id,
                Description = t.Description,
                Zone = t.Zone,
                Statut = t.Statut,
                Priorite = t.Priorite,
                Responsable = t.Responsable,
                DatePrevue = t.DatePrevue,
                DateRealisation = t.DateRealisation,
                Note = t.Note
            }).ToListAsync(cancellationToken);

        var escalesDisponibles = await dbContext.Escales
            .AsNoTracking()
            .OrderBy(e => e.Navire)
            .Select(e => new EscaleYardOptionDto { Id = e.Id, Navire = e.Navire })
            .ToListAsync(cancellationToken);

        var zonesDisponibles = await dbContext.ReferenceValues
            .AsNoTracking()
            .Where(r => r.ListKey == ReferenceListKeys.YardZone && r.IsActive)
            .OrderBy(r => r.SortOrder)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);

        return new YardPlannerDashboardDto
        {
            NaviresEnCours = naviresEnCours,
            VesselYardPlans = vesselYardPlans,
            TransfertsOut = transfertsOut,
            HousekeepingTasks = housekeepingTasks,
            EscalesDisponibles = escalesDisponibles,
            ZonesDisponibles = zonesDisponibles
        };
    }
}
