using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddVesselPlanningModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalContainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroConteneur = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LigneMaritime = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Sens = table.Column<int>(type: "integer", nullable: false),
                    Decision = table.Column<int>(type: "integer", nullable: false),
                    Commentaire = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ReferenceEmail = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DateDecisionUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DecidePar = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalContainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalContainers_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DangerousContainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroConteneur = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LigneMaritime = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ClasseImo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StatutBadt = table.Column<int>(type: "integer", nullable: false),
                    DateValiditeBadt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StatutOperationnel = table.Column<int>(type: "integer", nullable: false),
                    Commentaire = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangerousContainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DangerousContainers_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmptyContainerTargets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    LigneMaritime = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TypeConteneur = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    QuantiteSouhaitee = table.Column<int>(type: "integer", nullable: false),
                    QuantiteAjoutee = table.Column<int>(type: "integer", nullable: false),
                    QuantitePlanifiee = table.Column<int>(type: "integer", nullable: false),
                    QuantiteEmbarquee = table.Column<int>(type: "integer", nullable: false),
                    QuantiteCoupee = table.Column<int>(type: "integer", nullable: false),
                    MotifCoupure = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmptyContainerTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmptyContainerTargets_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationalIncidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Categorie = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Localisation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DateDebutUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateFinUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Statut = table.Column<int>(type: "integer", nullable: false),
                    Gravite = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ActionRealisee = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DeclarePar = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalIncidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationalIncidents_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalContainers_EscaleId",
                table: "AdditionalContainers",
                column: "EscaleId");

            migrationBuilder.CreateIndex(
                name: "IX_DangerousContainers_EscaleId",
                table: "DangerousContainers",
                column: "EscaleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmptyContainerTargets_EscaleId",
                table: "EmptyContainerTargets",
                column: "EscaleId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalIncidents_EscaleId",
                table: "OperationalIncidents",
                column: "EscaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalContainers");

            migrationBuilder.DropTable(
                name: "DangerousContainers");

            migrationBuilder.DropTable(
                name: "EmptyContainerTargets");

            migrationBuilder.DropTable(
                name: "OperationalIncidents");
        }
    }
}
