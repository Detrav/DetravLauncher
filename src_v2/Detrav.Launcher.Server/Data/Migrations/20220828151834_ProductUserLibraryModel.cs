using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class ProductUserLibraryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductModelProductUserModel");

            migrationBuilder.CreateTable(
                name: "ProductUserLibraries",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    IsOwner = table.Column<bool>(type: "bit", nullable: false),
                    IsRequest = table.Column<bool>(type: "bit", nullable: false),
                    IsWishlist = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUserLibraries", x => new { x.ProductId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProductUserLibraries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductUserLibraries_ProductUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ProductUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductUserLibraries_UserId",
                table: "ProductUserLibraries",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductUserLibraries");

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
    }
}
