using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddPriseDeConnaissance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PriseDeConnaissanceLeUtc",
                table: "ShiftHandoverNotes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriseDeConnaissanceParUtilisateur",
                table: "ShiftHandoverNotes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriseDeConnaissanceLeUtc",
                table: "ShiftHandoverNotes");

            migrationBuilder.DropColumn(
                name: "PriseDeConnaissanceParUtilisateur",
                table: "ShiftHandoverNotes");
        }
    }
}
