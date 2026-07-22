using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class AddYardPlanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HousekeepingTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    Priorite = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Responsable = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DatePrevue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRealisation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousekeepingTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransfertsOut",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bay = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NombreConteneurs = table.Column<int>(type: "int", nullable: false),
                    DetailOuDestination = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HeureDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HeureFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Commentaire = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransfertsOut", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VesselYardPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceMaritime = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ZoneDebarquement = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReefersImport = table.Column<int>(type: "int", nullable: false),
                    ReefersExport = table.Column<int>(type: "int", nullable: false),
                    ConteneursTransbordement = table.Column<int>(type: "int", nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VesselYardPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VesselYardPlans_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VesselYardPlans_EscaleId",
                table: "VesselYardPlans",
                column: "EscaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HousekeepingTasks");

            migrationBuilder.DropTable(
                name: "TransfertsOut");

            migrationBuilder.DropTable(
                name: "VesselYardPlans");
        }
    }
}
