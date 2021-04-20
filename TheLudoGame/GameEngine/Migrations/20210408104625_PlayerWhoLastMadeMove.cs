using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class PlayerWhoLastMadeMove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedTokens",
                table: "Player");

            migrationBuilder.AddColumn<int>(
                name: "PlayerIDLastMadeMove",
                table: "Board",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerIDLastMadeMove",
                table: "Board");

            migrationBuilder.AddColumn<int>(
                name: "FinishedTokens",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}