using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class more_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Files");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Files",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Collection",
                table: "Files",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Files_Path_Collection",
                table: "Files",
                columns: new[] { "Path", "Collection" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_Path_Collection",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Collection",
                table: "Files");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
