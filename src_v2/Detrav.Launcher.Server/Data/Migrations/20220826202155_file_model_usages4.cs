using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class file_model_usages4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Versions_ProductVersionModelId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ProductVersionModelId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Screenshots");

            migrationBuilder.DropColumn(
                name: "ProductVersionModelId",
                table: "Files");

            migrationBuilder.CreateTable(
                name: "ProductVersionFileModel",
                columns: table => new
                {
                    VerionId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    VersionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersionFileModel", x => new { x.FileId, x.VerionId });
                    table.ForeignKey(
                        name: "FK_ProductVersionFileModel_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVersionFileModel_Versions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "Versions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersionFileModel_VersionId",
                table: "ProductVersionFileModel",
                column: "VersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVersionFileModel");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Screenshots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductVersionModelId",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_ProductVersionModelId",
                table: "Files",
                column: "ProductVersionModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Versions_ProductVersionModelId",
                table: "Files",
                column: "ProductVersionModelId",
                principalTable: "Versions",
                principalColumn: "Id");
        }
    }
}
