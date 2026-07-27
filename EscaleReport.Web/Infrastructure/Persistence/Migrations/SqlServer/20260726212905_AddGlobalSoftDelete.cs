using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.SqlServer
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
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "VesselYardPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "VesselYardPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "TtVesselAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TtVesselAssignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TtVesselAssignments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "TtEffectifs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TtEffectifs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TtEffectifs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "TtDeconnexions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TtDeconnexions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TtDeconnexions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "TransfertsOut",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "TransfertsOut",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TransfertsOut",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "StsPointeurs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StsPointeurs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StsPointeurs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "StsIncidents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StsIncidents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StsIncidents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ShiftHandoverNotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ShiftHandoverNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ShiftHandoverNotes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RtgPannes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RtgPannes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RtgPannes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RtgEffectifs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RtgEffectifs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RtgEffectifs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RtgClashes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RtgClashes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RtgClashes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RopnEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RopnEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RopnEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ReportEmailLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportEmailLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportEmailLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "RemplacementsOperateur",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "RemplacementsOperateur",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RemplacementsOperateur",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "OperationalIncidents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "OperationalIncidents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OperationalIncidents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "IttTransfers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "IttTransfers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IttTransfers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "IttTransferIncidents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "IttTransferIncidents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IttTransferIncidents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "IttEquipementEffectifs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "IttEquipementEffectifs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IttEquipementEffectifs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "IttEnginPannes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "IttEnginPannes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IttEnginPannes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "HousekeepingTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "HousekeepingTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HousekeepingTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "GeneralSettings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "GeneralSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GeneralSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "GateTruckIssues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "GateTruckIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GateTruckIssues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "GantryAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "GantryAssignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GantryAssignments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "Gantries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Gantries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Gantries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "Escales",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Escales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Escales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "EscalePlanificationNotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EscalePlanificationNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EscalePlanificationNotes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "EnginProblemes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EnginProblemes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EnginProblemes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "EnginDeconnexions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EnginDeconnexions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EnginDeconnexions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "EmptyContainerTargets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EmptyContainerTargets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmptyContainerTargets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "EmailTemplates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "EmailTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmailTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "DangerousContainers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "DangerousContainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DangerousContainers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "CoordinatorModuleVisibilities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CoordinatorModuleVisibilities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoordinatorModuleVisibilities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "CoordinatorIncidents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CoordinatorIncidents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoordinatorIncidents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "ContainerAnomalies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ContainerAnomalies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ContainerAnomalies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "CargoConsommations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CargoConsommations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CargoConsommations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "AutresEnginsEffectifs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AutresEnginsEffectifs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AutresEnginsEffectifs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "AlertThresholds",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AlertThresholds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AlertThresholds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                table: "AdditionalContainers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AdditionalContainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AdditionalContainers",
                type: "bit",
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
