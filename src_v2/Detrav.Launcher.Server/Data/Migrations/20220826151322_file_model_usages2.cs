using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Detrav.Launcher.Server.Data.Migrations
{
    public partial class file_model_usages2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Screenshots");

            migrationBuilder.DropColumn(
                name: "Poster",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Blobs");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Achievements");

            migrationBuilder.RenameColumn(
                name: "MD5",
                table: "Blobs",
                newName: "Hash");

            migrationBuilder.AddColumn<int>(
                name: "DataId",
                table: "Screenshots",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "PosterId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Seek",
                table: "Blobs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "IconId",
                table: "Achievements",
                type: "int",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Seek",
                table: "Blobs");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "Achievements");

            migrationBuilder.RenameColumn(
                name: "Hash",
                table: "Blobs",
                newName: "MD5");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Screenshots",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Poster",
                table: "Products",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Blobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Achievements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Icon",
                table: "Achievements",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
