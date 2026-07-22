using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class AddContainerAnomalies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContainerAnomalies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EscaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroConteneur = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sens = table.Column<int>(type: "int", nullable: false),
                    LigneMaritime = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Raison = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    DateResolutionUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResoluPar = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Commentaire = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReferenceEchange = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerAnomalies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContainerAnomalies_Escales_EscaleId",
                        column: x => x.EscaleId,
                        principalTable: "Escales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceValues", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContainerAnomalies_EscaleId",
                table: "ContainerAnomalies",
                column: "EscaleId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceValues_ListKey_Value",
                table: "ReferenceValues",
                columns: new[] { "ListKey", "Value" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContainerAnomalies");

            migrationBuilder.DropTable(
                name: "ReferenceValues");
        }
    }
}
