using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddCargoControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CargoConsommations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    DischHazard = table.Column<int>(type: "integer", nullable: false),
                    DischReefer = table.Column<int>(type: "integer", nullable: false),
                    DischOog = table.Column<int>(type: "integer", nullable: false),
                    DischImport = table.Column<int>(type: "integer", nullable: false),
                    DischRestow = table.Column<int>(type: "integer", nullable: false),
                    DischTranshipment = table.Column<int>(type: "integer", nullable: false),
                    Disch20Pieds = table.Column<int>(type: "integer", nullable: false),
                    Disch40Pieds = table.Column<int>(type: "integer", nullable: false),
                    DischRealisee = table.Column<bool>(type: "boolean", nullable: false),
                    LoadYard = table.Column<int>(type: "integer", nullable: false),
                    LoadEnCommunication = table.Column<int>(type: "integer", nullable: false),
                    LoadRealisee = table.Column<bool>(type: "boolean", nullable: false),
                    RevisedLoadRecu = table.Column<bool>(type: "boolean", nullable: false),
                    RevisedLoadDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevisedLoadPar = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    RevisedLoadObservations = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoConsommations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CargoConsommations_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CargoConsommations_EscaleId",
                table: "CargoConsommations",
                column: "EscaleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargoConsommations");
        }
    }
}
