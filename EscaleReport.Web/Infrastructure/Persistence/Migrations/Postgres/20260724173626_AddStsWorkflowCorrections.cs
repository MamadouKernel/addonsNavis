using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddStsWorkflowCorrections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebutUtc",
                table: "RopnEntries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFinUtc",
                table: "RopnEntries",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDebutUtc",
                table: "RopnEntries");

            migrationBuilder.DropColumn(
                name: "DateFinUtc",
                table: "RopnEntries");
        }
    }
}
