# État d'implémentation — EscaleReport

Suivi de l'avancement par rapport au CDC. Légende : ✅ fait · 🟡 partiel · ⬜ non commencé.

| § CDC | Module | Statut | Détail |
|---|---|---|---|
| §2 | Utilisateurs, rôles, permissions | ✅ | Identity + rôles (dont Shift Manager, lecture seule + validation du rapport de shift) + permissions décorrélées des rôles (claims), UI admin "Comptes & permissions" (créer/désactiver/réinitialiser mot de passe/rôle/poste/équipe/permissions individuelles), journal d'audit consultable et filtrable (traçabilité automatique de toutes les commandes), 8 comptes de démo seedés |
| §4.1 | Escales — tableau de bord | ✅ | Liste + filtres |
| §4.2 | Escales — création | ✅ | Formulaire, brouillon tant que champs obligatoires manquants |
| §4.3 | Escales — statuts | ✅ | Changement de statut opérations/planification, passage "Terminées" réservé à la permission dédiée, alerte des points d'attention (anomalies/dangereux/additionnels non traités) à la clôture |
| §5.1 | Vessel Planning — Anomalies conteneurs | ✅ | |
| §5.2 | Vessel Planning — Conteneurs vides | ✅ | |
| §5.3 | Vessel Planning — Incidents opérationnels | ✅ | |
| §5.4 | Vessel Planning — Conteneurs additionnels | ✅ | |
| §5.5 | Vessel Planning — Marchandises dangereuses | ✅ | |
| §6.1 | Dispatch STS — sélection shift/date | ✅ | Sélecteur date+shift, navires en cours/attendus affichés automatiquement (shift paramétrable) |
| §6.2 | Dispatch STS — portiques | ✅ | Affectation, statut, fin d'affectation |
| §6.3 | Dispatch STS — incidents STS | ✅ | |
| §6.4 | Dispatch STS — pointeurs | ✅ | |
| §6.5 | Dispatch STS — ROPN | ✅ | |
| §7.1 | Dispatch TT — effectif | ✅ | |
| §7.2 | Dispatch TT — affectation par navire | ✅ | |
| §7.3 | Dispatch TT — déconnexions TT | ✅ | |
| §8.1 | Dispatch RTG — effectif | ✅ | |
| §8.2 | Dispatch RTG — consultation incidents STS | ✅ | Lecture seule |
| §8.3 | Dispatch RTG — pannes RTG | ✅ | |
| §8.4 | Dispatch RTG — clashs | ✅ | |
| §8.5 | Dispatch RTG — problèmes camions Gate | ✅ | |
| §9.1 | Dispatch autres engins — engins disponibles | ✅ | Retrait calculé automatiquement |
| §9.2 | Dispatch autres engins — problèmes d'engins | ✅ | |
| §9.3 | Dispatch autres engins — déconnexions/absences | ✅ | |
| §9.4 | Dispatch autres engins — remplacement opérateurs | ✅ | |
| §10 | Cargo Control | ✅ | Dashboard, consommation Disch/Load, Revised Load, alertes ; export PDF/Excel/email au niveau escale |
| §11.1 | Yard Planner — navires en cours à quai | ✅ | Lecture seule |
| §11.2 | Yard Planner — plans navire et zones de débarquement | ✅ | Zones paramétrables |
| §11.3 | Yard Planner — transferts Out | ✅ | |
| §11.4 | Yard Planner — housekeeping | ✅ | |
| §12.1-12.7 | Coordinateur — vue consolidée (Navires/STS/TT/RTG+autres engins/Cargo/Yard/ITT) | ✅ | Choix des modules affichés par l'admin non implémenté (dépend de §15) |
| §12.8 | Coordinateur — incidents propres | ✅ | |
| §13.1 | ITT Controller — suivi des transferts | ✅ | Restant/avancement calculés automatiquement |
| §13.2 | ITT Controller — incidents de transfert | ✅ | |
| §13.3 | ITT Controller — équipements ITT | ✅ | |
| §13.4 | ITT Controller — pannes engins de transfert | ✅ | |
| §14.1 | Rapport de fin de shift | ✅ | Sélection date/shift/navire, synthèses par poste, pannes/incidents transverses, actions en cours et points à transmettre (saisis par le Coordinateur), rubrique Planification en fin de document, PDF généré pour le navire courant ou tous les navires en cours |
| §14.2 | Rapport de fin d'escale | ✅ | Consolidation complète (PDF + Excel) : horaires/statut final, 5 sections Vessel Planning, incidents STS, Cargo, ressources STS/TT, difficultés/actions/points restants ouverts |
| §14.3 | Export PDF | ✅ | Mécanisme générique en place (QuestPDF) |
| §14.4 | Export Excel | ✅ | Mécanisme générique en place (ClosedXML) |
| §14.5 | Envoi par e-mail | ✅ | `mailto:` + journal d'audit |
| §15.1 | Paramétrage — listes, portiques, modèles d'e-mail, seuils d'alerte | ✅ | UI admin complète (5 onglets) : réglages généraux, 9 listes de référence (activation/ajout), portiques (ajout/suppression protégée par référence réelle — affectations et incidents STS), 4 modèles d'e-mail avec substitution de variables branchée sur l'envoi réel du rapport d'escale, seuils d'alerte (CRUD) |
| §15.2 | Personnalisation de l'interface (mode clair/sombre) | ✅ | Bascule clair/sombre persistante (cookie), pilotée par attribut `data-theme` plutôt que `prefers-color-scheme` (choix explicite par utilisateur, adapté à un poste Control Room partagé) ; composants partagés (cartes, boutons, badges, tableaux, formulaires) themés dans les deux modes. La bibliothèque de ~10 thèmes et les effets visuels avancés (animations, halos, confettis) du CDC restent explicitement optionnels ("pourra proposer" / "à titre indicatif") et ne sont pas repris |
| §16 | Notifications et alertes | ✅ | Moteur calculant en direct les 14 situations du CDC (escale incomplète, ATA manquant, rapport final manquant, incident critique, pannes RTG/autres engins/ITT, anomalies non résolues, additionnels sans décision, BADT à renouveler, Disch/Load/Revised Load, transferts ITT en retard, tâches Yard en retard, relève de shift manquante), page dédiée + envoi par e-mail (mailto). Teams explicitement hors périmètre CDC |
| §17 | Reporting et indicateurs transverses | ✅ | Tableau de bord d'indicateurs (escales, anomalies/incidents, disponibilité STS/TT/RTG/autres engins, pannes, taux d'affectation, conteneurs vides/additionnels/dangereux, réalisation Disch/Load, délai de production des rapports, transferts ITT, tâches Yard), filtrable par période/shift/navire/ligne/quai/type d'incident. Filtres équipement/utilisateur/équipe non repris (aucune dimension exploitable dans le modèle de données actuel pour ces axes) |
| §18 | Règles de gestion transverses (unicité, conflits, complétude, relève) | ✅ | Avertissement de doublon probable à la création d'escale (Vessel Visit ou navire+voyage+ETA, avec confirmation possible) ; conflits de concurrence convertis en message convivial (filtre global) au lieu d'une exception brute ; date/auteur de dernière mise à jour affichés sur la fiche escale ; indicateur de complétude toujours visible (brouillon, anomalies/incidents/additionnels/dangereux non traités) ; confirmation de "prise de connaissance" par le shift entrant sur le rapport de fin de shift |

## Identité visuelle

Palette "lagune" alignée sur le prototype HTML de référence (bleu-nuit du quai, turquoise lagon, corail pour les alertes) — pilotée par variables CSS pour rester cohérente en mode clair et sombre, appliquée aux composants partagés (cartes, boutons, badges, tableaux, sidebar en dégradé, page de connexion). Tableau de bord des escales converti en grille de cartes avec bordure d'accent colorée selon le statut opérationnel. Scène animée quai/navire/portique (CSS pur, `_PortScene.cshtml`) reprise sur la page de connexion et le tableau de bord des escales : un chariot transfère un conteneur du navire vers le parc en boucle, respecte `prefers-reduced-motion`. Les autres effets avancés du prototype (dix thèmes commutables, glisser-déposer interactif, confettis) restent hors périmètre — le CDC §15.2 les qualifie lui-même d'indicatifs, sans impact sur les règles de gestion.

## Sécurité

Audit interne (revue de code + tests manuels) suivi de corrections, toutes vérifiées en conditions réelles :

| Constat | Correctif |
|---|---|
| Cloisonnement par poste Dispatch absent (un Dispatcher pouvait agir sur STS/TT/RTG/Autres engins indépendamment de son poste assigné) | `ICurrentUserService.Poste` (claim posée à la connexion depuis `PosteParDefaut`) + `DispatchAccessControl.CanAccessPoste` vérifié dans les 26 handlers de commande Dispatch |
| Pas de HTTPS/HSTS ni d'en-têtes de sécurité | `UseHttpsRedirection`/`UseHsts` + CSP, X-Frame-Options, X-Content-Type-Options, Referrer-Policy, Permissions-Policy dans `Program.cs` |
| Cookie de session sans `Secure` forcé | `CookieSecurePolicy.Always` en dehors de Dev (`SameAsRequest` en Dev, pas d'endpoint https local) |
| Injection possible dans le `mailto:` du rapport (paramètre `cc=`/`bcc=` via le champ destinataires) | `LogReportEmailCommandValidator` interdit `?/&/#/=/%` dans chaque adresse |
| Mot de passe Postgres en clair dans `appsettings.json` (dépôt public) | Retiré, à fournir via `dotnet user-secrets` ou variable d'environnement |
| Tentatives de connexion échouées et accès refusés non journalisés | `AuditLoggingBehaviour` journalise les `ForbiddenAccessException` ; `AccountController.Login` journalise les échecs (compte inconnu, verrouillé, mot de passe) |
| Pages scaffold ASP.NET par défaut exposées sans usage (`Home/Index`, `Home/Privacy`) ; `ThemeController` sans `[Authorize]` | Pages supprimées (`Home/Error` conservé, requis par `UseExceptionHandler`) ; `[Authorize]` ajouté sur `ThemeController` |
| `Html.Raw()` sur du HTML construit par interpolation de chaîne (`Views/Statistics/Index.cshtml`) | Remplacé par un `TagBuilder`/`IHtmlContent` qui encode automatiquement |
| `ForbiddenAccessException`/`ValidationException` non interceptées : page d'erreur brute (pile d'appel visible) au lieu d'un message convivial | `AppExceptionFilter` (même mécanisme que `ConcurrencyExceptionFilter` déjà en place pour les conflits de concurrence) : message dans `TempData["Error"]` + redirection vers la page d'origine |

## Comptes de démonstration (environnement de développement)

Créés par le seeder au premier démarrage. Mot de passe fixé en développement (`appsettings.Development.json`) pour les 3 comptes les plus utilisés en démo ; les autres restent générés aléatoirement et affichés une seule fois dans les logs si non fournis via configuration :

| Compte | Rôle | Mot de passe (dev) |
|---|---|---|
| `admin` | Administrateur | `Bonjour@2027` |
| `vplanner` | Vessel Planner | `Bonjour@2027` |
| `dispatcher1` | Dispatcher | `Bonjour@2027` |
| `cargo1` | Cargo Controller | généré (voir logs) |
| `yardplanner1` | Yard Planner | généré (voir logs) |
| `coordinateur1` | Coordinateur Control Room | généré (voir logs) |
| `ittcontroller1` | ITT Controller | généré (voir logs) |
| `shiftmanager1` | Shift Manager | généré (voir logs) |
