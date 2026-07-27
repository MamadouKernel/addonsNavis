using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddGlobalSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "VesselYardPlans",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "VesselYardPlans",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "VesselYardPlans",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "TtVesselAssignments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TtVesselAssignments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TtVesselAssignments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "TtEffectifs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TtEffectifs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TtEffectifs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "TtDeconnexions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TtDeconnexions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TtDeconnexions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "TransfertsOut",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TransfertsOut",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TransfertsOut",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "StsPointeurs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StsPointeurs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StsPointeurs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "StsIncidents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StsIncidents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StsIncidents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ShiftHandoverNotes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ShiftHandoverNotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ShiftHandoverNotes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RtgPannes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RtgPannes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RtgPannes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RtgEffectifs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RtgEffectifs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RtgEffectifs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RtgClashes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RtgClashes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RtgClashes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RopnEntries",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RopnEntries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RopnEntries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ReportEmailLogs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportEmailLogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportEmailLogs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RemplacementsOperateur",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RemplacementsOperateur",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RemplacementsOperateur",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "OperationalIncidents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "OperationalIncidents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OperationalIncidents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "IttTransfers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "IttTransfers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IttTransfers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "IttTransferIncidents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "IttTransferIncidents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IttTransferIncidents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "IttEquipementEffectifs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "IttEquipementEffectifs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IttEquipementEffectifs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "IttEnginPannes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "IttEnginPannes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IttEnginPannes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "HousekeepingTasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "HousekeepingTasks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HousekeepingTasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "GeneralSettings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "GeneralSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GeneralSettings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "GateTruckIssues",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "GateTruckIssues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GateTruckIssues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "GantryAssignments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "GantryAssignments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GantryAssignments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "Gantries",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Gantries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Gantries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "Escales",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Escales",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Escales",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "EscalePlanificationNotes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EscalePlanificationNotes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EscalePlanificationNotes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "EnginProblemes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EnginProblemes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EnginProblemes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "EnginDeconnexions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EnginDeconnexions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EnginDeconnexions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "EmptyContainerTargets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EmptyContainerTargets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmptyContainerTargets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "EmailTemplates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EmailTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmailTemplates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "DangerousContainers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "DangerousContainers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DangerousContainers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "CoordinatorModuleVisibilities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CoordinatorModuleVisibilities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoordinatorModuleVisibilities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "CoordinatorIncidents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CoordinatorIncidents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoordinatorIncidents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ContainerAnomalies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ContainerAnomalies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ContainerAnomalies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "CargoConsommations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CargoConsommations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CargoConsommations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "AutresEnginsEffectifs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AutresEnginsEffectifs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AutresEnginsEffectifs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "AlertThresholds",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AlertThresholds",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AlertThresholds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "AdditionalContainers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AdditionalContainers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AdditionalContainers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "VesselYardPlans");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "VesselYardPlans");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "VesselYardPlans");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "TtVesselAssignments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "TtVesselAssignments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TtVesselAssignments");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "TtEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "TtEffectifs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TtEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "TtDeconnexions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "TtDeconnexions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TtDeconnexions");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "TransfertsOut");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "TransfertsOut");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TransfertsOut");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "StsPointeurs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "StsPointeurs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StsPointeurs");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "StsIncidents");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "StsIncidents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StsIncidents");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "ShiftHandoverNotes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ShiftHandoverNotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ShiftHandoverNotes");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "RtgPannes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RtgPannes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RtgPannes");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "RtgEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RtgEffectifs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RtgEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "RtgClashes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RtgClashes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RtgClashes");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "RopnEntries");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RopnEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RopnEntries");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "ReportEmailLogs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportEmailLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportEmailLogs");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "RemplacementsOperateur");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RemplacementsOperateur");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RemplacementsOperateur");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "OperationalIncidents");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "OperationalIncidents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OperationalIncidents");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "IttTransfers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "IttTransfers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "IttTransfers");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "IttTransferIncidents");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "IttTransferIncidents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "IttTransferIncidents");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "IttEquipementEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "IttEquipementEffectifs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "IttEquipementEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "IttEnginPannes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "IttEnginPannes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "IttEnginPannes");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "HousekeepingTasks");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "HousekeepingTasks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HousekeepingTasks");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "GateTruckIssues");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "GateTruckIssues");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GateTruckIssues");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "GantryAssignments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "GantryAssignments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GantryAssignments");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "Gantries");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Gantries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Gantries");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "Escales");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Escales");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Escales");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "EscalePlanificationNotes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EscalePlanificationNotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EscalePlanificationNotes");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "EnginProblemes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EnginProblemes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EnginProblemes");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "EnginDeconnexions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EnginDeconnexions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EnginDeconnexions");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "EmptyContainerTargets");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EmptyContainerTargets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmptyContainerTargets");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmailTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "DangerousContainers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DangerousContainers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DangerousContainers");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "CoordinatorModuleVisibilities");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CoordinatorModuleVisibilities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CoordinatorModuleVisibilities");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "CoordinatorIncidents");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CoordinatorIncidents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CoordinatorIncidents");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "ContainerAnomalies");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ContainerAnomalies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ContainerAnomalies");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "CargoConsommations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CargoConsommations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CargoConsommations");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "AutresEnginsEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AutresEnginsEffectifs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AutresEnginsEffectifs");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "AlertThresholds");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AlertThresholds");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AlertThresholds");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                table: "AdditionalContainers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AdditionalContainers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AdditionalContainers");
        }
    }
}
