using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class rework_file_system : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Files_IconId",
                table: "Achievements");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Files_PosterId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Files_DataId",
                table: "Screenshots");

            migrationBuilder.DropTable(
                name: "VersionFiles");

            migrationBuilder.DropIndex(
                name: "IX_Screenshots_DataId",
                table: "Screenshots");

            migrationBuilder.DropIndex(
                name: "IX_Products_PosterId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Achievements_IconId",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "DataId",
                table: "Screenshots");

            migrationBuilder.DropColumn(
                name: "PosterId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "Achievements");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Screenshots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosterFilePath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconFilePath",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_Collection",
                table: "Files",
                column: "Collection");

            migrationBuilder.CreateIndex(
                name: "IX_Files_Path",
                table: "Files",
                column: "Path");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_Collection",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_Path",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Screenshots");

            migrationBuilder.DropColumn(
                name: "PosterFilePath",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IconFilePath",
                table: "Achievements");

            migrationBuilder.AddColumn<int>(
                name: "DataId",
                table: "Screenshots",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PosterId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IconId",
                table: "Achievements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VersionFiles",
                columns: table => new
                {
                    FileId = table.Column<int>(type: "int", nullable: false),
                    VersionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionFiles", x => new { x.FileId, x.VersionId });
                    table.ForeignKey(
                        name: "FK_VersionFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersionFiles_Versions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "Versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Screenshots_DataId",
                table: "Screenshots",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PosterId",
                table: "Products",
                column: "PosterId");

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_IconId",
                table: "Achievements",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionFiles_VersionId",
                table: "VersionFiles",
                column: "VersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Files_IconId",
                table: "Achievements",
                column: "IconId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Files_PosterId",
                table: "Products",
                column: "PosterId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Screenshots_Files_DataId",
                table: "Screenshots",
                column: "DataId",
                principalTable: "Files",
                principalColumn: "Id");
        }
    }
}
