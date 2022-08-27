using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class version2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBeta",
                table: "Versions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Versions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBeta",
                table: "Versions");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Versions");
        }
    }
}
