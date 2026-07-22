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
