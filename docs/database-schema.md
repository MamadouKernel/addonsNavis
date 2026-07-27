# Correspondance du schéma de données

Le cahier de recette historique désigne douze ensembles fonctionnels avec des noms
préfixés par `er_*`. L'application actuelle utilise EF Core et répartit ces ensembles
dans des tables spécialisées. Les noms `er_*` ne constituent donc pas le contrat
physique de la base.

| Ensemble historique | Tables EF Core principales |
|---|---|
| Escales | `Escales` |
| Identité et habilitations | `AspNetUsers`, `AspNetRoles`, `UserPermissions` |
| Vessel Planning | `ContainerAnomalies`, `EmptyContainerTargets`, `OperationalIncidents`, `AdditionalContainers`, `DangerousContainers` |
| Dispatch STS | `Gantries`, `GantryAssignments`, `StsIncidents`, `StsPointeurs`, `RopnEntries` |
| Dispatch TT | `TtEffectifs`, `TtVesselAssignments`, `TtDeconnexions` |
| Dispatch RTG | `RtgEffectifs`, `RtgPannes`, `RtgClashes`, `GateTruckIssues` |
| Autres engins | `AutresEnginsEffectifs`, `EnginProblemes`, `EnginDeconnexions`, `RemplacementsOperateur` |
| Cargo | `CargoConsommations`, `ReportEmailLogs` |
| Yard (`er_dispatch_yard`) | `VesselYardPlans`, `TransfertsOut`, `HousekeepingTasks` |
| ITT (`er_dispatch_itt`) | `IttTransfers`, `IttTransferIncidents`, `IttEquipementEffectifs`, `IttEnginPannes` |
| Coordination (`er_coord`) | `CoordinatorIncidents`, `CoordinatorModuleVisibilities` |
| Reporting et paramètres | `ShiftHandoverNotes`, `EscalePlanificationNotes`, `GeneralSettings`, `EmailTemplates`, `AlertThresholds`, `AuditLogEntries` |

## Règle de recette

`PREP-11` doit vérifier que les ensembles fonctionnels et leurs tables EF Core sont
créés et accessibles après application des migrations. Il ne doit pas exiger que les
tables portent littéralement les anciens noms `er_*`.

Si une intégration externe exige ultérieurement ces anciens noms, privilégier des vues
SQL de compatibilité. Renommer les tables EF Core existantes modifierait le contrat de
persistance et nécessiterait des migrations coordonnées pour SQL Server et PostgreSQL.
