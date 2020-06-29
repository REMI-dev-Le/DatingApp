using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class PublicID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photod_Users_userId",
                table: "Photod");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Photod",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Photod_userId",
                table: "Photod",
                newName: "IX_Photod_UserId");

            migrationBuilder.AddColumn<string>(
                name: "PublicID",
                table: "Photod",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Photod_Users_UserId",
                table: "Photod",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photod_Users_UserId",
                table: "Photod");

            migrationBuilder.DropColumn(
                name: "PublicID",
                table: "Photod");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Photod",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Photod_UserId",
                table: "Photod",
                newName: "IX_Photod_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photod_Users_userId",
                table: "Photod",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
