using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddDispatchAutresEngins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutresEnginsEffectifs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisponibleReachStackers = table.Column<int>(type: "integer", nullable: false),
                    DisponibleEmptyHandlers = table.Column<int>(type: "integer", nullable: false),
                    DisponibleAutres = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutresEnginsEffectifs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnginDeconnexions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Engin = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateDebutUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateRetourUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Motif = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnginDeconnexions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnginProblemes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Engin = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Categorie = table.Column<int>(type: "integer", nullable: false),
                    DateDebutUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateFinUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Probleme = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RetireEffectif = table.Column<bool>(type: "boolean", nullable: false),
                    ActionRealisee = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnginProblemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RemplacementsOperateur",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Operateur = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EnginQuitte = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NouvelEngin = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateHeureUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Raison = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Commentaire = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemplacementsOperateur", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutresEnginsEffectifs");

            migrationBuilder.DropTable(
                name: "EnginDeconnexions");

            migrationBuilder.DropTable(
                name: "EnginProblemes");

            migrationBuilder.DropTable(
                name: "RemplacementsOperateur");
        }
    }
}
