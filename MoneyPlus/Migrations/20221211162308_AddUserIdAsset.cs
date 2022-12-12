using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyPlus.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_UserId",
                table: "Assets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AspNetUsers_UserId",
                table: "Assets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AspNetUsers_UserId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_UserId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Assets");
        }
    }
}
