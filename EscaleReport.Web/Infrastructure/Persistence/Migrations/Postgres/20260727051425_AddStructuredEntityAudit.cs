using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddStructuredEntityAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangesJson",
                table: "AuditLogEntries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntityId",
                table: "AuditLogEntries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "AuditLogEntries",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogEntries_EntityType_EntityId",
                table: "AuditLogEntries",
                columns: new[] { "EntityType", "EntityId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditLogEntries_EntityType_EntityId",
                table: "AuditLogEntries");

            migrationBuilder.DropColumn(
                name: "ChangesJson",
                table: "AuditLogEntries");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "AuditLogEntries");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "AuditLogEntries");
        }
    }
}
