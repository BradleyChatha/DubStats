using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class PackageDependencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackageDependencies",
                columns: table => new
                {
                    PackageDependencyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DependantPackageId = table.Column<int>(nullable: false),
                    DependencyPackageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageDependencies", x => x.PackageDependencyId);
                    table.ForeignKey(
                        name: "FK_PackageDependencies_Packages_DependantPackageId",
                        column: x => x.DependantPackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageDependencies_Packages_DependencyPackageId",
                        column: x => x.DependencyPackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageDependencies_DependencyPackageId",
                table: "PackageDependencies",
                column: "DependencyPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageDependencies_DependantPackageId_DependencyPackageId",
                table: "PackageDependencies",
                columns: new[] { "DependantPackageId", "DependencyPackageId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageDependencies");
        }
    }
}
