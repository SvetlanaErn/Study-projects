using Microsoft.EntityFrameworkCore.Migrations;

namespace GameEngine.Migrations
{
    public partial class Properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Square_Boards_BoardId",
                table: "Square");

            migrationBuilder.DropForeignKey(
                name: "FK_Square_Tokens_OccupantId",
                table: "Square");

            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Players_PlayerId",
                table: "Tokens");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Tokens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OccupantId",
                table: "Square",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BoardId",
                table: "Square",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Square_Boards_BoardId",
                table: "Square",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Square_Tokens_OccupantId",
                table: "Square",
                column: "OccupantId",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Players_PlayerId",
                table: "Tokens",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Square_Boards_BoardId",
                table: "Square");

            migrationBuilder.DropForeignKey(
                name: "FK_Square_Tokens_OccupantId",
                table: "Square");

            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Players_PlayerId",
                table: "Tokens");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Tokens",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OccupantId",
                table: "Square",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BoardId",
                table: "Square",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Square_Boards_BoardId",
                table: "Square",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Square_Tokens_OccupantId",
                table: "Square",
                column: "OccupantId",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Players_PlayerId",
                table: "Tokens",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}