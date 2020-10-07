using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class PackageUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackageUpdates",
                columns: table => new
                {
                    ScheduledPackageUpdateId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WeekId = table.Column<int>(nullable: false),
                    PackageId = table.Column<int>(nullable: false),
                    Milestone = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageUpdates", x => x.ScheduledPackageUpdateId);
                    table.ForeignKey(
                        name: "FK_PackageUpdates_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageUpdates_Weeks_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Weeks",
                        principalColumn: "WeekId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageUpdates_WeekId",
                table: "PackageUpdates",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageUpdates_PackageId_WeekId_Milestone",
                table: "PackageUpdates",
                columns: new[] { "PackageId", "WeekId", "Milestone" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageUpdates");
        }
    }
}
