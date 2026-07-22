using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class AddDispatchRtg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GateTruckIssues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeOperation = table.Column<int>(type: "int", nullable: false),
                    CamionReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateDebutUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFinUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProblemeRencontre = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ActionRealisee = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GateTruckIssues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RtgClashes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lieu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EnginsConcernes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateDebutUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFinUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ActionRealisee = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RtgClashes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RtgEffectifs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalParc = table.Column<int>(type: "int", nullable: false),
                    Disponible = table.Column<int>(type: "int", nullable: false),
                    Affecte = table.Column<int>(type: "int", nullable: false),
                    EnPanne = table.Column<int>(type: "int", nullable: false),
                    Retire = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RtgEffectifs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RtgPannes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Engin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateDebutUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFinUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Raison = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RetireEffectif = table.Column<bool>(type: "bit", nullable: false),
                    CommentaireReprise = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RtgPannes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GateTruckIssues");

            migrationBuilder.DropTable(
                name: "RtgClashes");

            migrationBuilder.DropTable(
                name: "RtgEffectifs");

            migrationBuilder.DropTable(
                name: "RtgPannes");
        }
    }
}
