using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class TokenID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Square_Token_OccupantId",
                table: "Square");

            migrationBuilder.RenameColumn(
                name: "OccupantId",
                table: "Square",
                newName: "TokenId");

            migrationBuilder.RenameIndex(
                name: "IX_Square_OccupantId",
                table: "Square",
                newName: "IX_Square_TokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Square_Token_TokenId",
                table: "Square",
                column: "TokenId",
                principalTable: "Token",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Square_Token_TokenId",
                table: "Square");

            migrationBuilder.RenameColumn(
                name: "TokenId",
                table: "Square",
                newName: "OccupantId");

            migrationBuilder.RenameIndex(
                name: "IX_Square_TokenId",
                table: "Square",
                newName: "IX_Square_OccupantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Square_Token_OccupantId",
                table: "Square",
                column: "OccupantId",
                principalTable: "Token",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}