using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Cargo;
using EscaleReport.Web.Domain.Escales;
using EscaleReport.Web.Domain.VesselPlanning;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Infrastructure.Persistence;

internal static class RecipeDataSeeder
{
    public static async Task SeedAsync(IApplicationDbContext db)
    {
        const string marker = "RECETTE-";
        if (await db.Escales.AnyAsync(e => e.VesselVisit != null && e.VesselVisit.StartsWith(marker)))
        {
            return;
        }

        var now = DateTime.UtcNow;
        var escales = new[]
        {
            Escale("RECETTE-01", "ABIDJAN STAR", "V001", now.AddHours(-8), StatutOperations.EnCours, StatutPlanification.PlanValide, now.AddHours(-7)),
            Escale("RECETTE-02", "LAGOON EXPRESS", "V002", now.AddHours(-2), StatutOperations.EnCours, StatutPlanification.Planifie, now.AddHours(-1)),
            Escale("RECETTE-03", "GULF TRADER", "V003", now.AddHours(-4), StatutOperations.PasEncoreDebutees, StatutPlanification.NonPlanifie),
            Escale("RECETTE-04", "OCEAN BRIDGE", "V004", now.AddHours(10), StatutOperations.PasEncoreDebutees, StatutPlanification.Planifie),
            Escale("RECETTE-05", "COASTAL SPIRIT", "V005", now.AddDays(-2), StatutOperations.Terminees, StatutPlanification.PlanValide, now.AddDays(-2).AddHours(1)),
            Escale("RECETTE-06", "ATLANTIC HOPE", "V006", now.AddDays(-1), StatutOperations.Terminees, StatutPlanification.PlanValide, now.AddDays(-1).AddHours(1))
        };
        db.Escales.AddRange(escales);

        var working = escales[0];
        for (var i = 1; i <= 3; i++)
        {
            db.ContainerAnomalies.Add(new ContainerAnomaly
            {
                EscaleId = working.Id, NumeroConteneur = $"RECANO{i:D4}",
                Sens = Sens.Debarquement, LigneMaritime = working.LigneMaritime,
                Position = $"Bay {i:D2}", Raison = i == 1 ? "Introuvable" : "Avarie",
                Statut = i == 3 ? AnomalyStatus.Resolu : AnomalyStatus.NonResolu
            });
            db.EmptyContainerTargets.Add(new EmptyContainerTarget
            {
                EscaleId = working.Id, LigneMaritime = working.LigneMaritime,
                TypeConteneur = i % 2 == 0 ? "40 pieds" : "20 pieds",
                QuantiteSouhaitee = 10 + i, QuantiteAjoutee = i, QuantitePlanifiee = 8,
                QuantiteEmbarquee = 5, QuantiteCoupee = i - 1,
                MotifCoupure = i > 1 ? "Capacité navire" : null
            });
            db.OperationalIncidents.Add(new OperationalIncident
            {
                EscaleId = working.Id, Categorie = i == 1 ? "Portique" : "Sécurité",
                Localisation = $"Quai {i}", DateDebutUtc = now.AddHours(-i),
                DateFinUtc = i == 3 ? null : now.AddMinutes(-20 * i),
                Statut = i == 3 ? IncidentStatus.EnCours : IncidentStatus.Resolu,
                Gravite = i == 2 ? IncidentGravite.Critique : IncidentGravite.Moyen,
                Description = $"Incident de recette {i}", DeclarePar = "seed-recette"
            });
            db.AdditionalContainers.Add(new AdditionalContainer
            {
                EscaleId = working.Id, NumeroConteneur = $"RECADD{i:D4}",
                LigneMaritime = working.LigneMaritime, Position = $"Bay {i + 3:D2}",
                Sens = Sens.Embarquement,
                Decision = i == 3 ? AdditionalContainerDecision.EnAttente : AdditionalContainerDecision.Embarque,
                Commentaire = "Donnée de recette"
            });
            db.DangerousContainers.Add(new DangerousContainer
            {
                EscaleId = working.Id, NumeroConteneur = $"RECDNG{i:D4}",
                LigneMaritime = working.LigneMaritime, ClasseImo = i == 1 ? "3" : "8",
                Position = $"Bay {i + 6:D2}",
                StatutBadt = i == 1 ? BadtStatus.ARenouveler : BadtStatus.Pris,
                DateValiditeBadt = i == 1 ? now.AddDays(-1) : now.AddDays(5),
                StatutOperationnel = DangerousContainerStatus.ASuivre,
                Commentaire = "Donnée de recette"
            });
        }

        db.CargoConsommations.AddRange(
            new CargoConsommation
            {
                EscaleId = escales[0].Id, DischImport = 24, DischRestow = 3,
                DischTranshipment = 7, DischRealisee = true, LoadYard = 18,
                LoadEnCommunication = 2, LoadRealisee = true, RevisedLoadRecu = true,
                RevisedLoadDateUtc = now, RevisedLoadPar = "seed-recette"
            },
            new CargoConsommation
            {
                EscaleId = escales[1].Id, DischImport = 15, DischRestow = 2,
                DischTranshipment = 4, DischRealisee = false, LoadYard = 12,
                LoadEnCommunication = 3, LoadRealisee = false, RevisedLoadRecu = false
            });

        await db.SaveChangesAsync(CancellationToken.None);
    }

    private static Escale Escale(
        string visit, string navire, string voyage, DateTime eta,
        StatutOperations operations, StatutPlanification planification,
        DateTime? ata = null) => new()
    {
        VesselVisit = visit, Navire = navire, Voyage = voyage,
        LigneMaritime = "RECETTE LINE", Quai = "Poste 1", Eta = eta, Ata = ata,
        Etc = operations == StatutOperations.Terminees ? eta.AddHours(12) : eta.AddHours(18),
        Shift = "Matin", StatutOperations = operations,
        StatutPlanification = planification, Planificateur = "seed-recette", IsDraft = false
    };
}
