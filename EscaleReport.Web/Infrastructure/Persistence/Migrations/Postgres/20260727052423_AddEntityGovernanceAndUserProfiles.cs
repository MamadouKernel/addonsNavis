using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddEntityGovernanceAndUserProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "VesselYardPlans",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "VesselYardPlans",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "VesselYardPlans",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "VesselYardPlans",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "TtVesselAssignments",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "TtVesselAssignments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "TtVesselAssignments",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "TtVesselAssignments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "TtEffectifs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "TtEffectifs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "TtEffectifs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "TtEffectifs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "TtDeconnexions",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "TtDeconnexions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "TtDeconnexions",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "TtDeconnexions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "TransfertsOut",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "TransfertsOut",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "TransfertsOut",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "TransfertsOut",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "StsPointeurs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "StsPointeurs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "StsPointeurs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "StsPointeurs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "StsIncidents",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "StsIncidents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "StsIncidents",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "StsIncidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "ShiftHandoverNotes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "ShiftHandoverNotes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "ShiftHandoverNotes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "ShiftHandoverNotes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "RtgPannes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "RtgPannes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "RtgPannes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "RtgPannes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "RtgEffectifs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "RtgEffectifs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "RtgEffectifs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "RtgEffectifs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "RtgClashes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "RtgClashes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "RtgClashes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "RtgClashes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "RopnEntries",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "RopnEntries",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "RopnEntries",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "RopnEntries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "ReportEmailLogs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "ReportEmailLogs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "ReportEmailLogs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "ReportEmailLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "RemplacementsOperateur",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "RemplacementsOperateur",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "RemplacementsOperateur",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "RemplacementsOperateur",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "OperationalIncidents",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "OperationalIncidents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "OperationalIncidents",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "OperationalIncidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "IttTransfers",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "IttTransfers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "IttTransfers",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "IttTransfers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "IttTransferIncidents",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "IttTransferIncidents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "IttTransferIncidents",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "IttTransferIncidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "IttEquipementEffectifs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "IttEquipementEffectifs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "IttEquipementEffectifs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "IttEquipementEffectifs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "IttEnginPannes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "IttEnginPannes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "IttEnginPannes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "IttEnginPannes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "HousekeepingTasks",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "HousekeepingTasks",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "HousekeepingTasks",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "HousekeepingTasks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "GeneralSettings",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "GeneralSettings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "GeneralSettings",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "GeneralSettings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "GateTruckIssues",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "GateTruckIssues",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "GateTruckIssues",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "GateTruckIssues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "GantryAssignments",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "GantryAssignments",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "GantryAssignments",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "GantryAssignments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "Gantries",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "Gantries",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "Gantries",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "Gantries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "Escales",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "Escales",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "Escales",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "Escales",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "EscalePlanificationNotes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "EscalePlanificationNotes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "EscalePlanificationNotes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "EscalePlanificationNotes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "EnginProblemes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "EnginProblemes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "EnginProblemes",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "EnginProblemes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "EnginDeconnexions",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "EnginDeconnexions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "EnginDeconnexions",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "EnginDeconnexions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "EmptyContainerTargets",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "EmptyContainerTargets",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "EmptyContainerTargets",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "EmptyContainerTargets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "EmailTemplates",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "EmailTemplates",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "EmailTemplates",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "EmailTemplates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "DangerousContainers",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "DangerousContainers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "DangerousContainers",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "DangerousContainers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "CoordinatorModuleVisibilities",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "CoordinatorModuleVisibilities",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "CoordinatorModuleVisibilities",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "CoordinatorModuleVisibilities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "CoordinatorIncidents",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "CoordinatorIncidents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "CoordinatorIncidents",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "CoordinatorIncidents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "ContainerAnomalies",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "ContainerAnomalies",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "ContainerAnomalies",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "ContainerAnomalies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "CargoConsommations",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "CargoConsommations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "CargoConsommations",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "CargoConsommations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "AutresEnginsEffectifs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "AutresEnginsEffectifs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "AutresEnginsEffectifs",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "AutresEnginsEffectifs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateEntree",
                table: "AspNetUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateExpirationCompteUtc",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fonction",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Matricule",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prenoms",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsableHierarchique",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteAffectation",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Societe",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "AlertThresholds",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "AlertThresholds",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "AlertThresholds",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "AlertThresholds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "DataSource",
                table: "AdditionalContainers",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "AdditionalContainers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifecycleStatus",
                table: "AdditionalContainers",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Version",
                table: "AdditionalContainers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "VesselYardPlans");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "VesselYardPlans");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "VesselYardPlans");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "VesselYardPlans");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "TtVesselAssignments");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "TtVesselAssignments");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "TtVesselAssignments");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "TtVesselAssignments");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "TtEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "TtEffectifs");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "TtEffectifs");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "TtEffectifs");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "TtDeconnexions");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "TtDeconnexions");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "TtDeconnexions");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "TtDeconnexions");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "TransfertsOut");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "TransfertsOut");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "TransfertsOut");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "TransfertsOut");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "StsPointeurs");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "StsPointeurs");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "StsPointeurs");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "StsPointeurs");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "StsIncidents");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "StsIncidents");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "StsIncidents");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "StsIncidents");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "ShiftHandoverNotes");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "ShiftHandoverNotes");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "ShiftHandoverNotes");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "ShiftHandoverNotes");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "RtgPannes");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "RtgPannes");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "RtgPannes");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "RtgPannes");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "RtgEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "RtgEffectifs");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "RtgEffectifs");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "RtgEffectifs");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "RtgClashes");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "RtgClashes");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "RtgClashes");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "RtgClashes");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "RopnEntries");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "RopnEntries");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "RopnEntries");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "RopnEntries");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "ReportEmailLogs");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "ReportEmailLogs");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "ReportEmailLogs");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "ReportEmailLogs");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "RemplacementsOperateur");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "RemplacementsOperateur");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "RemplacementsOperateur");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "RemplacementsOperateur");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "OperationalIncidents");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "OperationalIncidents");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "OperationalIncidents");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "OperationalIncidents");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "IttTransfers");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "IttTransfers");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "IttTransfers");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "IttTransfers");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "IttTransferIncidents");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "IttTransferIncidents");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "IttTransferIncidents");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "IttTransferIncidents");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "IttEquipementEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "IttEquipementEffectifs");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "IttEquipementEffectifs");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "IttEquipementEffectifs");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "IttEnginPannes");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "IttEnginPannes");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "IttEnginPannes");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "IttEnginPannes");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "HousekeepingTasks");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "HousekeepingTasks");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "HousekeepingTasks");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "HousekeepingTasks");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "GateTruckIssues");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "GateTruckIssues");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "GateTruckIssues");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "GateTruckIssues");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "GantryAssignments");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "GantryAssignments");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "GantryAssignments");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "GantryAssignments");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "Gantries");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "Gantries");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "Gantries");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Gantries");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "Escales");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "Escales");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "Escales");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Escales");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "EscalePlanificationNotes");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "EscalePlanificationNotes");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "EscalePlanificationNotes");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "EscalePlanificationNotes");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "EnginProblemes");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "EnginProblemes");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "EnginProblemes");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "EnginProblemes");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "EnginDeconnexions");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "EnginDeconnexions");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "EnginDeconnexions");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "EnginDeconnexions");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "EmptyContainerTargets");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "EmptyContainerTargets");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "EmptyContainerTargets");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "EmptyContainerTargets");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "DangerousContainers");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "DangerousContainers");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "DangerousContainers");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "DangerousContainers");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "CoordinatorModuleVisibilities");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "CoordinatorModuleVisibilities");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "CoordinatorModuleVisibilities");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "CoordinatorModuleVisibilities");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "CoordinatorIncidents");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "CoordinatorIncidents");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "CoordinatorIncidents");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "CoordinatorIncidents");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "ContainerAnomalies");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "ContainerAnomalies");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "ContainerAnomalies");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "ContainerAnomalies");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "CargoConsommations");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "CargoConsommations");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "CargoConsommations");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "CargoConsommations");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "AutresEnginsEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "AutresEnginsEffectifs");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "AutresEnginsEffectifs");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "AutresEnginsEffectifs");

            migrationBuilder.DropColumn(
                name: "DateEntree",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateExpirationCompteUtc",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Fonction",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Matricule",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Prenoms",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResponsableHierarchique",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SiteAffectation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Societe",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "AlertThresholds");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "AlertThresholds");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "AlertThresholds");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "AlertThresholds");

            migrationBuilder.DropColumn(
                name: "DataSource",
                table: "AdditionalContainers");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "AdditionalContainers");

            migrationBuilder.DropColumn(
                name: "LifecycleStatus",
                table: "AdditionalContainers");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "AdditionalContainers");
        }
    }
}
