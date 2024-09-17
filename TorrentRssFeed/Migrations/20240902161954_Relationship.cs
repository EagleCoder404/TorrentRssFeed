using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorrentRssFeed.Migrations
{
    /// <inheritdoc />
    public partial class Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Torrent_TorrentList_TorrentListId",
                table: "Torrent");

            migrationBuilder.AlterColumn<int>(
                name: "TorrentListId",
                table: "Torrent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Torrent_TorrentList_TorrentListId",
                table: "Torrent",
                column: "TorrentListId",
                principalTable: "TorrentList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Torrent_TorrentList_TorrentListId",
                table: "Torrent");

            migrationBuilder.AlterColumn<int>(
                name: "TorrentListId",
                table: "Torrent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Torrent_TorrentList_TorrentListId",
                table: "Torrent",
                column: "TorrentListId",
                principalTable: "TorrentList",
                principalColumn: "Id");
        }
    }
}
