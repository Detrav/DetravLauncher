using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class optimization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVersionFileModel_Files_FileId",
                table: "ProductVersionFileModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVersionFileModel_Versions_VersionId",
                table: "ProductVersionFileModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVersionFileModel",
                table: "ProductVersionFileModel");

            migrationBuilder.RenameTable(
                name: "ProductVersionFileModel",
                newName: "VersionFiles");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVersionFileModel_VersionId",
                table: "VersionFiles",
                newName: "IX_VersionFiles_VersionId");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "Versions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ApiKey",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VersionFiles",
                table: "VersionFiles",
                columns: new[] { "FileId", "VersionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Versions_Version",
                table: "Versions",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ApiKey",
                table: "Products",
                column: "ApiKey");

            migrationBuilder.AddForeignKey(
                name: "FK_VersionFiles_Files_FileId",
                table: "VersionFiles",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VersionFiles_Versions_VersionId",
                table: "VersionFiles",
                column: "VersionId",
                principalTable: "Versions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VersionFiles_Files_FileId",
                table: "VersionFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_VersionFiles_Versions_VersionId",
                table: "VersionFiles");

            migrationBuilder.DropIndex(
                name: "IX_Versions_Version",
                table: "Versions");

            migrationBuilder.DropIndex(
                name: "IX_Products_ApiKey",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VersionFiles",
                table: "VersionFiles");

            migrationBuilder.RenameTable(
                name: "VersionFiles",
                newName: "ProductVersionFileModel");

            migrationBuilder.RenameIndex(
                name: "IX_VersionFiles_VersionId",
                table: "ProductVersionFileModel",
                newName: "IX_ProductVersionFileModel_VersionId");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "Versions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ApiKey",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVersionFileModel",
                table: "ProductVersionFileModel",
                columns: new[] { "FileId", "VersionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVersionFileModel_Files_FileId",
                table: "ProductVersionFileModel",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVersionFileModel_Versions_VersionId",
                table: "ProductVersionFileModel",
                column: "VersionId",
                principalTable: "Versions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
