using System.Security.Cryptography;
using EscaleReport.Web.Application.Common.Interfaces;
using EscaleReport.Web.Domain.Common;
using EscaleReport.Web.Domain.Dispatch;
using EscaleReport.Web.Domain.Identity;
using EscaleReport.Web.Domain.Settings;
using EscaleReport.Web.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EscaleReport.Web.Infrastructure.Persistence;

// Réservé au développement (voir Program.cs) : ne s'exécute jamais en Production.
// Les mots de passe de seed ne sont jamais codés en dur (CDC §27) — ils viennent de
// configuration (user-secrets / variables d'environnement) ou, à défaut, sont générés
// aléatoirement et affichés une seule fois dans les logs de démarrage.
public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        var dbContext = services.GetRequiredService<IApplicationDbContext>();
        await SeedAnomalyReasonsAsync(dbContext);
        await SeedIncidentCategoriesAsync(dbContext);
        await SeedStsIncidentTypesAsync(dbContext);
        await SeedGantriesAsync(dbContext);
        await SeedYardZonesAsync(dbContext);
        await SeedShiftsAsync(dbContext);
        await SeedQuaisAsync(dbContext);
        await SeedLignesMaritimesAsync(dbContext);
        await SeedBaysAsync(dbContext);
        await SeedEmailTemplatesAsync(dbContext);

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        if (await userManager.Users.AnyAsync())
        {
            await SeedConfiguredItAdminAsync(userManager, dbContext, logger, configuration);
            return; // des comptes existent déjà, on ne touche à rien.
        }

        await SeedUserAsync(
            userManager, dbContext, logger, configuration,
            configKeyPrefix: "SeedAdmin", defaultUserName: "admin",
            role: Roles.Administrateur, permissions: []); // l'admin a déjà toutes les permissions via son rôle

        // Compte de démonstration pour le rôle Vessel Planner (CDC §3), avec permissions
        // décorrélées du rôle comme le prévoit le CDC §2.1 — pas de bypass admin ici.
        await SeedUserAsync(
            userManager, dbContext, logger, configuration,
            configKeyPrefix: "SeedVesselPlanner", defaultUserName: "vplanner",
            role: Roles.VesselPlanner,
            permissions:
            [
                Permissions.ConsulterEscales,
                Permissions.CreerEscale,
                Permissions.ModifierEscale,
                Permissions.SaisirDonneesModule,
                Permissions.ModifierDonneesModule,
                Permissions.GenererPdf
            ]);

        // Compte de démonstration pour le rôle Dispatcher (CDC §2.2), sur le poste STS.
        await SeedUserAsync(
            userManager, dbContext, logger, configuration,
            configKeyPrefix: "SeedDispatcher", defaultUserName: "dispatcher1",
            role: Roles.Dispatcher,
            permissions:
            [
                Permissions.ConsulterEscales,
                Permissions.SaisirDonneesModule,
                Permissions.ModifierDonneesModule
            ],
            poste: "STS");

        // Compte de démonstration pour le rôle Cargo Controller (CDC §10).
        await SeedUserAsync(
            userManager, dbContext, logger, configuration,
            configKeyPrefix: "SeedCargoController", defaultUserName: "cargo1",
            role: Roles.CargoController,
            permissions:
            [
                Permissions.ConsulterEscales,
                Permissions.SaisirDonneesModule,
                Permissions.GenererPdf,
                Permissions.GenererExcel,
                Permissions.EnvoyerRapportEmail
            ]);

        // Compte de démonstration pour le rôle Yard Planner (CDC §11).
        await SeedUserAsync(
            userManager, dbContext, logger, configuration,
            configKeyPrefix: "SeedYardPlanner", defaultUserName: "yardplanner1",
            role: Roles.YardPlanner,
            permissions:
            [
                Permissions.ConsulterEscales,
                Permissions.SaisirDonneesModule,
                Permissions.ModifierDonneesModule
            ]);

        // Compte de démonstration pour le rôle Coordinateur Control Room (CDC §12).
        await SeedUserAsync(
            userManager, dbContext, logger, configuration,
            configKeyPrefix: "SeedCoordinateur", defaultUserName: "coordinateur1",
            role: Roles.CoordinateurControlRoom,
            permissions:
            [
                Permissions.ConsulterEscales,
                Permissions.ConsulterAutresModules,
                Permissions.SaisirDonneesModule,
                Permissions.ModifierDonneesModule
            ]);

        // Compte de démonstration pour le rôle ITT Controller (CDC §13).
        await SeedUserAsync(
            userManager, dbContext, logger, configuration,
            configKeyPrefix: "SeedIttController", defaultUserName: "ittcontroller1",
            role: Roles.IttController,
            permissions:
            [
                Permissions.ConsulterEscales,
                Permissions.SaisirDonneesModule,
                Permissions.ModifierDonneesModule
            ]);

        // Compte de démonstration pour le rôle Shift Manager (CDC §2) : accès lecture seule
        // à la situation consolidée du shift, sans intervenir dans la saisie opérationnelle.
        await SeedUserAsync(
            userManager, dbContext, logger, configuration,
            configKeyPrefix: "SeedShiftManager", defaultUserName: "shiftmanager1",
            role: Roles.ShiftManager,
            permissions:
            [
                Permissions.ConsulterEscales,
                Permissions.ConsulterAutresModules,
                Permissions.ConsulterRapportGeneral,
                Permissions.ValiderRapport,
                Permissions.ConsulterStatistiques
            ]);

        await SeedConfiguredItAdminAsync(userManager, dbContext, logger, configuration);
    }

    private static async Task SeedConfiguredItAdminAsync(
        UserManager<ApplicationUser> userManager, IApplicationDbContext dbContext,
        ILogger logger, IConfiguration configuration)
    {
        var email = configuration["SeedItAdmin:Email"];
        var password = configuration["SeedItAdmin:Password"];
        var userName = configuration["SeedItAdmin:UserName"] ?? "itadmin";
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return;

        var enableTwoFactor = configuration.GetValue("Security:RequireItAdminMfa", true);
        var existing = await userManager.FindByNameAsync(userName);
        if (existing is not null)
        {
            if (!enableTwoFactor)
            {
                existing.Email = email;
                existing.EmailConfirmed = true;
                existing.TwoFactorEnabled = false;
                await userManager.UpdateAsync(existing);
                if (!await userManager.IsInRoleAsync(existing, Roles.AdministrateurIT))
                    await userManager.AddToRoleAsync(existing, Roles.AdministrateurIT);
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(existing);
                await userManager.ResetPasswordAsync(existing, resetToken, password);
            }
            return;
        }

        await SeedUserAsync(userManager, dbContext, logger, configuration,
            "SeedItAdmin", userName, Roles.AdministrateurIT, [], email: email, enableTwoFactor: enableTwoFactor);
    }

    private static async Task SeedAnomalyReasonsAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.ReferenceValues.AnyAsync(r => r.ListKey == ReferenceListKeys.AnomalyReason))
        {
            return;
        }

        string[] raisons = ["Introuvable", "Engagé", "Surarrimé", "Roulé", "Avarie", "Refusé", "Autre"];
        for (var i = 0; i < raisons.Length; i++)
        {
            dbContext.ReferenceValues.Add(new ReferenceValue
            {
                ListKey = ReferenceListKeys.AnomalyReason,
                Value = raisons[i],
                SortOrder = i
            });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedIncidentCategoriesAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.ReferenceValues.AnyAsync(r => r.ListKey == ReferenceListKeys.IncidentCategory))
        {
            return;
        }

        string[] categories = ["Portique", "Navire", "Yard", "Engin", "Système informatique", "Sécurité", "Autre"];
        for (var i = 0; i < categories.Length; i++)
        {
            dbContext.ReferenceValues.Add(new ReferenceValue
            {
                ListKey = ReferenceListKeys.IncidentCategory,
                Value = categories[i],
                SortOrder = i
            });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedStsIncidentTypesAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.ReferenceValues.AnyAsync(r => r.ListKey == ReferenceListKeys.StsIncidentType))
        {
            return;
        }

        string[] types =
        [
            "Panne spreader", "Panne électrique", "Panne mécanique", "Panne automate",
            "Problème de communication", "Arrêt sécurité", "Attente navire", "Autre"
        ];
        for (var i = 0; i < types.Length; i++)
        {
            dbContext.ReferenceValues.Add(new ReferenceValue
            {
                ListKey = ReferenceListKeys.StsIncidentType,
                Value = types[i],
                SortOrder = i
            });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedGantriesAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.Gantries.AnyAsync())
        {
            return;
        }

        for (var i = 1; i <= 8; i++)
        {
            dbContext.Gantries.Add(new Gantry { Code = $"CR{i}" });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedYardZonesAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.ReferenceValues.AnyAsync(r => r.ListKey == ReferenceListKeys.YardZone))
        {
            return;
        }

        string[] zones = ["Zone A", "Zone B", "Zone C", "Zone D", "Zone Reefer", "Zone Transbordement"];
        for (var i = 0; i < zones.Length; i++)
        {
            dbContext.ReferenceValues.Add(new ReferenceValue
            {
                ListKey = ReferenceListKeys.YardZone,
                Value = zones[i],
                SortOrder = i
            });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedShiftsAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.ReferenceValues.AnyAsync(r => r.ListKey == ReferenceListKeys.Shift))
        {
            return;
        }

        // CDC §1 "Glossaire" : "Les shifts concernés sont notamment le matin, l'après-midi et
        // la nuit" — liste paramétrable, ces 3 valeurs ne sont qu'un socle par défaut.
        string[] shifts = ["Matin", "Après-midi", "Nuit"];
        for (var i = 0; i < shifts.Length; i++)
        {
            dbContext.ReferenceValues.Add(new ReferenceValue
            {
                ListKey = ReferenceListKeys.Shift,
                Value = shifts[i],
                SortOrder = i
            });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedQuaisAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.ReferenceValues.AnyAsync(r => r.ListKey == ReferenceListKeys.Quai))
        {
            return;
        }

        string[] quais = ["Poste 1", "Poste 2", "Poste 3", "Poste 4"];
        for (var i = 0; i < quais.Length; i++)
        {
            dbContext.ReferenceValues.Add(new ReferenceValue { ListKey = ReferenceListKeys.Quai, Value = quais[i], SortOrder = i });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedLignesMaritimesAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.ReferenceValues.AnyAsync(r => r.ListKey == ReferenceListKeys.LigneMaritime))
        {
            return;
        }

        string[] lignes = ["CMA CGM", "Maersk", "MSC", "PIL", "Grimaldi"];
        for (var i = 0; i < lignes.Length; i++)
        {
            dbContext.ReferenceValues.Add(new ReferenceValue { ListKey = ReferenceListKeys.LigneMaritime, Value = lignes[i], SortOrder = i });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedBaysAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.ReferenceValues.AnyAsync(r => r.ListKey == ReferenceListKeys.Bay))
        {
            return;
        }

        for (var i = 1; i <= 10; i++)
        {
            dbContext.ReferenceValues.Add(new ReferenceValue { ListKey = ReferenceListKeys.Bay, Value = $"Bay {i:D2}", SortOrder = i - 1 });
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedEmailTemplatesAsync(IApplicationDbContext dbContext)
    {
        if (await dbContext.EmailTemplates.AnyAsync())
        {
            return;
        }

        dbContext.EmailTemplates.Add(new EmailTemplate
        {
            Cle = EmailTemplateKeys.RapportEscale,
            Sujet = "Rapport d'escale - {navire} ({voyage})",
            Corps = "Bonjour,\n\nVeuillez trouver ci-joint le rapport d'escale du navire {navire}, voyage {voyage}, ligne {ligne}.\n\n" +
                    "Merci de joindre le fichier PDF exporté depuis la fiche escale avant l'envoi.\n\nCordialement."
        });
        dbContext.EmailTemplates.Add(new EmailTemplate
        {
            Cle = EmailTemplateKeys.RapportShift,
            Sujet = "Rapport de fin de shift - {date} ({shift})",
            Corps = "Bonjour,\n\nVeuillez trouver ci-joint le rapport de fin de shift du {date}, shift {shift}.\n\n" +
                    "Merci de joindre le fichier PDF exporté depuis l'écran Rapport de shift avant l'envoi.\n\nCordialement."
        });
        dbContext.EmailTemplates.Add(new EmailTemplate
        {
            Cle = EmailTemplateKeys.ConsommationDisch,
            Sujet = "Consommation Disch - {navire} ({voyage})",
            Corps = "Bonjour,\n\nVeuillez trouver ci-joint la consommation Disch du navire {navire}, voyage {voyage}.\n\nCordialement."
        });
        dbContext.EmailTemplates.Add(new EmailTemplate
        {
            Cle = EmailTemplateKeys.ConsommationLoad,
            Sujet = "Consommation Load - {navire} ({voyage})",
            Corps = "Bonjour,\n\nVeuillez trouver ci-joint la consommation Load du navire {navire}, voyage {voyage}.\n\nCordialement."
        });

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    private static async Task SeedUserAsync(
        UserManager<ApplicationUser> userManager,
        IApplicationDbContext dbContext,
        ILogger logger,
        IConfiguration configuration,
        string configKeyPrefix,
        string defaultUserName,
        string role,
        IReadOnlyList<string> permissions,
        string? poste = null,
        string? email = null,
        bool enableTwoFactor = false)
    {
        var userName = configuration[$"{configKeyPrefix}:UserName"] ?? defaultUserName;
        var password = configuration[$"{configKeyPrefix}:Password"];

        if (string.IsNullOrWhiteSpace(password))
        {
            password = GenerateRandomPassword();
            logger.LogWarning(
                "Aucun mot de passe configuré ({ConfigKey}:Password). Compte '{UserName}' créé avec un mot " +
                "de passe généré : {Password} — à changer immédiatement après la première connexion.",
                configKeyPrefix, userName, password);
        }

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email ?? $"{userName}@escalereport.local",
            EmailConfirmed = true,
            TwoFactorEnabled = enableTwoFactor,
            IsActive = true,
            PosteParDefaut = poste
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            logger.LogError("Échec de la création du compte '{UserName}' : {Errors}", userName, errors);
            return;
        }

        await userManager.AddToRoleAsync(user, role);

        foreach (var permission in permissions)
        {
            dbContext.UserPermissions.Add(new UserPermission { UserId = user.Id, PermissionKey = permission });
        }

        if (permissions.Count > 0)
        {
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }

        logger.LogInformation("Compte '{UserName}' créé avec le rôle {Role}.", userName, role);
    }

    private static string GenerateRandomPassword()
    {
        // 20 caractères tirés d'un alphabet élargi : longueur/complexité largement au-dessus
        // de la politique de mot de passe définie dans DependencyInjection. Un tirage
        // purement aléatoire sur cet alphabet peut par malchance ne produire aucun caractère
        // spécial et violer silencieusement la politique (RequireNonAlphanumeric) — un des
        // caractères est donc forcé dans chaque catégorie requise, puis le tout est mélangé.
        const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lower = "abcdefghijkmnopqrstuvwxyz";
        const string digits = "23456789";
        const string symbols = "!@#$%";
        const string allowed = upper + lower + digits + symbols;

        var chars = new char[20];
        chars[0] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
        chars[1] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
        chars[2] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
        chars[3] = symbols[RandomNumberGenerator.GetInt32(symbols.Length)];

        Span<byte> buffer = stackalloc byte[16];
        RandomNumberGenerator.Fill(buffer);
        for (var i = 0; i < buffer.Length; i++)
        {
            chars[4 + i] = allowed[buffer[i] % allowed.Length];
        }

        // Mélange Fisher-Yates pour ne pas laisser les 4 premiers caractères prévisibles.
        for (var i = chars.Length - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }
        return new string(chars);
    }
}
