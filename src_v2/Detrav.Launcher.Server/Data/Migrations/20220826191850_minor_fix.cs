using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class minor_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AchievementModelProductUserModel",
                columns: table => new
                {
                    AchievementsId = table.Column<int>(type: "int", nullable: false),
                    ProductUsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementModelProductUserModel", x => new { x.AchievementsId, x.ProductUsersId });
                    table.ForeignKey(
                        name: "FK_AchievementModelProductUserModel_Achievements_AchievementsId",
                        column: x => x.AchievementsId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementModelProductUserModel_ProductUsers_ProductUsersId",
                        column: x => x.ProductUsersId,
                        principalTable: "ProductUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AchievementModelProductUserModel_ProductUsersId",
                table: "AchievementModelProductUserModel",
                column: "ProductUsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AchievementModelProductUserModel");
        }
    }
}
