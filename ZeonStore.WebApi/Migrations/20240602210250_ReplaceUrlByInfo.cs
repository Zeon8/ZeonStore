using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeonStore.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceUrlByInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ExebutableFile",
                table: "Applications");

            migrationBuilder.CreateTable(
                name: "DownloadFileInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DownloadUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Platform = table.Column<string>(type: "TEXT", nullable: false),
                    ExebutableName = table.Column<string>(type: "TEXT", nullable: false),
                    ServerApplicationDetailedInfoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadFileInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DownloadFileInfo_Applications_ServerApplicationDetailedInfoId",
                        column: x => x.ServerApplicationDetailedInfoId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadFileInfo_ServerApplicationDetailedInfoId",
                table: "DownloadFileInfo",
                column: "ServerApplicationDetailedInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadFileInfo");

            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "Applications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExebutableFile",
                table: "Applications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
