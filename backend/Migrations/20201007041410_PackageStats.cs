using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class PackageStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    PackageId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.PackageId);
                });

            migrationBuilder.CreateTable(
                name: "PackageStats",
                columns: table => new
                {
                    PackageStatsId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Downloads = table.Column<int>(nullable: false),
                    Stars = table.Column<int>(nullable: false),
                    Watchers = table.Column<int>(nullable: false),
                    Forks = table.Column<int>(nullable: false),
                    Issues = table.Column<int>(nullable: false),
                    HasBeenModified = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageStats", x => x.PackageStatsId);
                });

            migrationBuilder.CreateTable(
                name: "Weeks",
                columns: table => new
                {
                    WeekId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WeekStart = table.Column<DateTime>(nullable: false),
                    WeekEnd = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weeks", x => x.WeekId);
                });

            migrationBuilder.CreateTable(
                name: "WeekInfos",
                columns: table => new
                {
                    PackageWeekInfoId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PackageId = table.Column<int>(nullable: false),
                    WeekId = table.Column<int>(nullable: false),
                    PackageStatsAtStartId = table.Column<int>(nullable: false),
                    PackageStatsAtEndId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekInfos", x => x.PackageWeekInfoId);
                    table.ForeignKey(
                        name: "FK_WeekInfos_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeekInfos_PackageStats_PackageStatsAtEndId",
                        column: x => x.PackageStatsAtEndId,
                        principalTable: "PackageStats",
                        principalColumn: "PackageStatsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeekInfos_PackageStats_PackageStatsAtStartId",
                        column: x => x.PackageStatsAtStartId,
                        principalTable: "PackageStats",
                        principalColumn: "PackageStatsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeekInfos_Weeks_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Weeks",
                        principalColumn: "WeekId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_Name",
                table: "Packages",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeekInfos_PackageStatsAtEndId",
                table: "WeekInfos",
                column: "PackageStatsAtEndId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekInfos_PackageStatsAtStartId",
                table: "WeekInfos",
                column: "PackageStatsAtStartId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekInfos_WeekId",
                table: "WeekInfos",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekInfos_PackageId_WeekId",
                table: "WeekInfos",
                columns: new[] { "PackageId", "WeekId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weeks_WeekEnd",
                table: "Weeks",
                column: "WeekEnd",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weeks_WeekStart",
                table: "Weeks",
                column: "WeekStart",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeekInfos");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "PackageStats");

            migrationBuilder.DropTable(
                name: "Weeks");
        }
    }
}
