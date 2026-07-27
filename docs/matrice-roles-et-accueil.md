# Matrice des rôles et accueil personnalisé

L’accueil applique les mêmes règles que la navigation et les contrôleurs. Un raccourci n’est affiché que si le rôle **et** la permission individuelle donnent réellement accès au module.

| Rôle | Priorités à l’accueil | Accès complémentaires attendus |
|---|---|---|
| Administrateur | Comptes et équipes, paramétrage, audit, indicateurs | Tous les modules métier autorisés |
| Vessel Planner | Escales, alertes | Détail et planification navire |
| Dispatcher | Poste actif (STS, TT, RTG ou autres engins), changement de poste | Escales selon permission |
| Cargo Controller | Cargo Control, alertes, escales | Contrôle Disch/Load/Revised |
| Yard Planner | Yard Planning | Plans yard, transferts et housekeeping |
| ITT Controller | Flux ITT | Transferts, incidents et équipements |
| Coordinateur Control Room | Coordination, alertes, rapport de shift | Vue transverse selon permission |
| Shift Manager | Rapport de shift, indicateurs | Validation et analyse |

Les permissions restent vérifiées côté serveur. L’interface ne constitue jamais à elle seule une autorisation.
