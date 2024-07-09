using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeonStore.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpdateId",
                table: "DownloadFileInfo",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Update",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    ServerApplicationDetailedInfoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Update", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Update_Applications_ServerApplicationDetailedInfoId",
                        column: x => x.ServerApplicationDetailedInfoId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadFileInfo_UpdateId",
                table: "DownloadFileInfo",
                column: "UpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Update_ServerApplicationDetailedInfoId",
                table: "Update",
                column: "ServerApplicationDetailedInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DownloadFileInfo_Update_UpdateId",
                table: "DownloadFileInfo",
                column: "UpdateId",
                principalTable: "Update",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DownloadFileInfo_Update_UpdateId",
                table: "DownloadFileInfo");

            migrationBuilder.DropTable(
                name: "Update");

            migrationBuilder.DropIndex(
                name: "IX_DownloadFileInfo_UpdateId",
                table: "DownloadFileInfo");

            migrationBuilder.DropColumn(
                name: "UpdateId",
                table: "DownloadFileInfo");
        }
    }
}
