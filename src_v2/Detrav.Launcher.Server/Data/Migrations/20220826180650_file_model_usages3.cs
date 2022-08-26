using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class file_model_usages3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blobs_Files_FileId",
                table: "Blobs");

            migrationBuilder.DropIndex(
                name: "IX_Blobs_FileId",
                table: "Blobs");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Blobs");

            migrationBuilder.DropColumn(
                name: "Seek",
                table: "Blobs");

            migrationBuilder.CreateTable(
                name: "FileBlobs",
                columns: table => new
                {
                    BlobId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    Seek = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileBlobs", x => new { x.FileId, x.BlobId });
                    table.ForeignKey(
                        name: "FK_FileBlobs_Blobs_BlobId",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileBlobs_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileBlobs_BlobId",
                table: "FileBlobs",
                column: "BlobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileBlobs");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Blobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Seek",
                table: "Blobs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Blobs_FileId",
                table: "Blobs",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blobs_Files_FileId",
                table: "Blobs",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
