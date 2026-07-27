using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscaleReport.Web.Infrastructure.Persistence.Migrations.SqlServer;

// Migration d'alignement uniquement : les colonnes de gouvernance ont déjà été créées par
// AddEntityGovernanceAndUserProfiles. Le snapshot associé fixe l'état du modèle EF.
public partial class ReconcileEntityGovernanceModel : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) { }
    protected override void Down(MigrationBuilder migrationBuilder) { }
}
