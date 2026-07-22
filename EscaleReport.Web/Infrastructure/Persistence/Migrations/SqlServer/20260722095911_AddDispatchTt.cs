using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class AddDispatchTt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TtEffectifs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalParc = table.Column<int>(type: "int", nullable: false),
                    Designes = table.Column<int>(type: "int", nullable: false),
                    RaisonNonDesignation = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Retires = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TtEffectifs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TtVesselAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombrePrevu = table.Column<int>(type: "int", nullable: false),
                    NombreAffecte = table.Column<int>(type: "int", nullable: false),
                    NombreOperationnel = table.Column<int>(type: "int", nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TtVesselAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TtVesselAssignments_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TtVesselAssignments_EscaleId",
                table: "TtVesselAssignments",
                column: "EscaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TtEffectifs");

            migrationBuilder.DropTable(
                name: "TtVesselAssignments");
        }
    }
}
