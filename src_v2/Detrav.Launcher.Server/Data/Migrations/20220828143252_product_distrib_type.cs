using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class product_distrib_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Versions");

            migrationBuilder.AddColumn<int>(
                name: "DistributionType",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistributionType",
                table: "Products");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Versions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
