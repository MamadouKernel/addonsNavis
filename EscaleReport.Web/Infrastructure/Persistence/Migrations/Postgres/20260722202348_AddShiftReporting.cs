using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddShiftReporting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EscalePlanificationNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Commentaire = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscalePlanificationNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EscalePlanificationNotes_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftHandoverNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Shift = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ActionsEnCours = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    PointsATransmettre = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftHandoverNotes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EscalePlanificationNotes_EscaleId",
                table: "EscalePlanificationNotes",
                column: "EscaleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShiftHandoverNotes_Date_Shift",
                table: "ShiftHandoverNotes",
                columns: new[] { "Date", "Shift" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EscalePlanificationNotes");

            migrationBuilder.DropTable(
                name: "ShiftHandoverNotes");
        }
    }
}
