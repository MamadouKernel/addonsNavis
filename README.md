# EscaleReport

Application de gestion des escales et rapports de fin de shift (Côte d'Ivoire Terminal), développée selon le CDC EscaleReport.

## Stack technique

- .NET 10 / ASP.NET Core MVC — projet unique, Clean Architecture en dossiers (`Domain` / `Application` / `Infrastructure` / `Web`)
- CQRS via MediatR + FluentValidation
- EF Core 10, base de données configurable (SQL Server ou PostgreSQL) via `DatabaseProvider` dans `appsettings.json`
- ASP.NET Core Identity, permissions décorrélées des rôles (claims)
- Tailwind CSS v4 (design system en composants réutilisables)
- QuestPDF (export PDF) / ClosedXML (export Excel)

## Démarrage

```bash
cd EscaleReport.Web
dotnet run
```

La base est migrée et seedée automatiquement en environnement `Development`. Les comptes de démonstration et le mot de passe généré apparaissent dans les logs au premier démarrage (voir [STATUS.md](STATUS.md)).

## Choisir le fournisseur de base de données

Dans `appsettings.json` ou via variable d'environnement :

```json
{ "DatabaseProvider": "SqlServer" } // ou "Postgres"
```

## État d'avancement

Voir [STATUS.md](STATUS.md) pour le détail module par module par rapport au CDC.
# Administrateur IT et MFA par e-mail

Le rôle `AdministrateurIT` possède tous les accès fonctionnels et techniques. Son authentification à deux facteurs est obligatoire : après validation du mot de passe, un code à usage unique est envoyé à l'adresse e-mail confirmée du compte.

Les secrets ne doivent pas être ajoutés aux fichiers `appsettings*.json`. Configurez-les avec les variables d'environnement suivantes (syntaxe ASP.NET Core avec `__`) :

```text
AuthenticationEmail__Host=votre-domaine-com.mail.protection.outlook.com
AuthenticationEmail__Port=25
AuthenticationEmail__EnableSsl=true
AuthenticationEmail__FromAddress=escalereport@exemple.ci
AuthenticationEmail__FromName=EscaleReport
AuthenticationEmail__UserName=escalereport@exemple.ci
AuthenticationEmail__Password=<secret SMTP>
SeedItAdmin__UserName=itadmin
SeedItAdmin__Email=itadmin@exemple.ci
SeedItAdmin__Password=<mot de passe initial robuste>
```

Le compte initial n'est créé en développement que si ses trois valeurs `SeedItAdmin` sont fournies. Ensuite, seul un Administrateur IT peut créer ou modifier un autre compte de ce niveau.

En développement local uniquement, `Security__RequireItAdminMfa=false` permet de tester le compte IT sans serveur SMTP. Ne définissez jamais cette valeur à `false` en recette ou en production : l'absence de configuration conserve la valeur sécurisée par défaut (`true`).

Pour Microsoft 365 en relais SMTP sur le port 25, utilisez le point de terminaison MX de votre domaine (`*.mail.protection.outlook.com`) et autorisez l'adresse IP publique du serveur EscaleReport dans le connecteur Exchange Online. Laissez `UserName` et `Password` vides pour un relais authentifié par adresse IP. `EnableSsl=true` active STARTTLS.
