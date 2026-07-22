using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddDispatchSts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gantries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Statut = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gantries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GantryAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GantryId = table.Column<Guid>(type: "uuid", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    HeureDebut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HeureFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TacheOuZone = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Statut = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GantryAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GantryAssignments_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GantryAssignments_Gantries_GantryId",
                        column: x => x.GantryId,
                        principalTable: "Gantries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gantries_Code",
                table: "Gantries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GantryAssignments_EscaleId",
                table: "GantryAssignments",
                column: "EscaleId");

            migrationBuilder.CreateIndex(
                name: "IX_GantryAssignments_GantryId",
                table: "GantryAssignments",
                column: "GantryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GantryAssignments");

            migrationBuilder.DropTable(
                name: "Gantries");
        }
    }
}
