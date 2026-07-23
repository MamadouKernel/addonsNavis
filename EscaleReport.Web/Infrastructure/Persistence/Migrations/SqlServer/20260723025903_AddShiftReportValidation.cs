using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class AddShiftReportValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommentaireValidation",
                table: "ShiftHandoverNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValideLeUtc",
                table: "ShiftHandoverNotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidePar",
                table: "ShiftHandoverNotes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentaireValidation",
                table: "ShiftHandoverNotes");

            migrationBuilder.DropColumn(
                name: "ValideLeUtc",
                table: "ShiftHandoverNotes");

            migrationBuilder.DropColumn(
                name: "ValidePar",
                table: "ShiftHandoverNotes");
        }
    }
}
