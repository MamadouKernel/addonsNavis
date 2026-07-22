using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddStsIncidentsPointeursRopn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RopnEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Role = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DifficulteRencontree = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ActionRealisee = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Statut = table.Column<int>(type: "integer", nullable: false),
                    Commentaire = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RopnEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StsIncidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    GantryId = table.Column<Guid>(type: "uuid", nullable: true),
                    TypeIncident = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateDebutUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateFinUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Cause = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ConditionsReprise = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RetirePortiqueEffectif = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StsIncidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StsIncidents_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StsIncidents_Gantries_GantryId",
                        column: x => x.GantryId,
                        principalTable: "Gantries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StsPointeurs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Role = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NavireOuZone = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    HeurePriseDePosteUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HeureFinUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remarque = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StsPointeurs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StsIncidents_EscaleId",
                table: "StsIncidents",
                column: "EscaleId");

            migrationBuilder.CreateIndex(
                name: "IX_StsIncidents_GantryId",
                table: "StsIncidents",
                column: "GantryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RopnEntries");

            migrationBuilder.DropTable(
                name: "StsIncidents");

            migrationBuilder.DropTable(
                name: "StsPointeurs");
        }
    }
}
