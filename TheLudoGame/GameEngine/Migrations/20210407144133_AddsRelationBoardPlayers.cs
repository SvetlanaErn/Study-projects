using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class AddsRelationBoardPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Player_BoardId",
                table: "Player",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Board_BoardId",
                table: "Player",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Board_BoardId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_BoardId",
                table: "Player");
        }
    }
}