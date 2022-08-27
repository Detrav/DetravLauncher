using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class file_model_usages5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVersionFileModel_Versions_VersionId",
                table: "ProductVersionFileModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVersionFileModel",
                table: "ProductVersionFileModel");

            migrationBuilder.DropColumn(
                name: "VerionId",
                table: "ProductVersionFileModel");

            migrationBuilder.AlterColumn<int>(
                name: "VersionId",
                table: "ProductVersionFileModel",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVersionFileModel",
                table: "ProductVersionFileModel",
                columns: new[] { "FileId", "VersionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVersionFileModel_Versions_VersionId",
                table: "ProductVersionFileModel",
                column: "VersionId",
                principalTable: "Versions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVersionFileModel_Versions_VersionId",
                table: "ProductVersionFileModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVersionFileModel",
                table: "ProductVersionFileModel");

            migrationBuilder.AlterColumn<int>(
                name: "VersionId",
                table: "ProductVersionFileModel",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VerionId",
                table: "ProductVersionFileModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVersionFileModel",
                table: "ProductVersionFileModel",
                columns: new[] { "FileId", "VerionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVersionFileModel_Versions_VersionId",
                table: "ProductVersionFileModel",
                column: "VersionId",
                principalTable: "Versions",
                principalColumn: "Id");
        }
    }
}
