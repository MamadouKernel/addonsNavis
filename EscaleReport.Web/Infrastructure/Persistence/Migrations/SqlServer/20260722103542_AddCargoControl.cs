using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.SqlServer
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DischHazard = table.Column<int>(type: "int", nullable: false),
                    DischReefer = table.Column<int>(type: "int", nullable: false),
                    DischOog = table.Column<int>(type: "int", nullable: false),
                    DischImport = table.Column<int>(type: "int", nullable: false),
                    DischRestow = table.Column<int>(type: "int", nullable: false),
                    DischTranshipment = table.Column<int>(type: "int", nullable: false),
                    Disch20Pieds = table.Column<int>(type: "int", nullable: false),
                    Disch40Pieds = table.Column<int>(type: "int", nullable: false),
                    DischRealisee = table.Column<bool>(type: "bit", nullable: false),
                    LoadYard = table.Column<int>(type: "int", nullable: false),
                    LoadEnCommunication = table.Column<int>(type: "int", nullable: false),
                    LoadRealisee = table.Column<bool>(type: "bit", nullable: false),
                    RevisedLoadRecu = table.Column<bool>(type: "bit", nullable: false),
                    RevisedLoadDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisedLoadPar = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RevisedLoadObservations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
