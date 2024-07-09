using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeonStore.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddReleaseDateAndLatestUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DownloadFileInfo_Applications_ServerApplicationDetailedInfoId",
                table: "DownloadFileInfo");

            migrationBuilder.RenameColumn(
                name: "ServerApplicationDetailedInfoId",
                table: "DownloadFileInfo",
                newName: "InstallId");

            migrationBuilder.RenameIndex(
                name: "IX_DownloadFileInfo_ServerApplicationDetailedInfoId",
                table: "DownloadFileInfo",
                newName: "IX_DownloadFileInfo_InstallId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Update",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LatestUpdateId",
                table: "Applications",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_LatestUpdateId",
                table: "Applications",
                column: "LatestUpdateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Update_LatestUpdateId",
                table: "Applications",
                column: "LatestUpdateId",
                principalTable: "Update",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DownloadFileInfo_Update_InstallId",
                table: "DownloadFileInfo",
                column: "InstallId",
                principalTable: "Update",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Update_LatestUpdateId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_DownloadFileInfo_Update_InstallId",
                table: "DownloadFileInfo");

            migrationBuilder.DropIndex(
                name: "IX_Applications_LatestUpdateId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Update");

            migrationBuilder.DropColumn(
                name: "LatestUpdateId",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "InstallId",
                table: "DownloadFileInfo",
                newName: "ServerApplicationDetailedInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_DownloadFileInfo_InstallId",
                table: "DownloadFileInfo",
                newName: "IX_DownloadFileInfo_ServerApplicationDetailedInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DownloadFileInfo_Applications_ServerApplicationDetailedInfoId",
                table: "DownloadFileInfo",
                column: "ServerApplicationDetailedInfoId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
