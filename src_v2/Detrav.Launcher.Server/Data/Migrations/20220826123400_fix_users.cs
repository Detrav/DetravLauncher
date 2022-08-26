using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class fix_users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductUsers_Products_ProductId",
                table: "ProductUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProductUsers_ProductId",
                table: "ProductUsers");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductUsers");

            migrationBuilder.CreateTable(
                name: "ProductModelProductUserModel",
                columns: table => new
                {
                    ProductUsersId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductModelProductUserModel", x => new { x.ProductUsersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductModelProductUserModel_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductModelProductUserModel_ProductUsers_ProductUsersId",
                        column: x => x.ProductUsersId,
                        principalTable: "ProductUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductModelProductUserModel_ProductsId",
                table: "ProductModelProductUserModel",
                column: "ProductsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductModelProductUserModel");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductUsers_ProductId",
                table: "ProductUsers",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductUsers_Products_ProductId",
                table: "ProductUsers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
