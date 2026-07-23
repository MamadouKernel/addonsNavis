using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.SqlServer
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
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriseDeConnaissanceParUtilisateur",
                table: "ShiftHandoverNotes",
                type: "nvarchar(max)",
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
