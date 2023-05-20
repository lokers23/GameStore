using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.DAL.Migrations
{
    public partial class ChangeActivationFromKeyToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_key_activation_id",
                table: "key");

            migrationBuilder.DropIndex(
                name: "activation_id",
                table: "key");

            migrationBuilder.DropIndex(
                name: "name2",
                table: "game");

            migrationBuilder.DropColumn(
                name: "activation_id",
                table: "key");

            migrationBuilder.AddColumn<int>(
                name: "activation_id",
                table: "game",
                type: "int",
                nullable: true,
                defaultValueSql: "NULL");

            migrationBuilder.CreateIndex(
                name: "activation_id",
                table: "game",
                column: "activation_id");

            migrationBuilder.CreateIndex(
                name: "name2",
                table: "game",
                column: "name");

            migrationBuilder.AddForeignKey(
                name: "FK_game_activation_id",
                table: "game",
                column: "activation_id",
                principalTable: "activation",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_game_activation_id",
                table: "game");

            migrationBuilder.DropIndex(
                name: "activation_id",
                table: "game");

            migrationBuilder.DropIndex(
                name: "name2",
                table: "game");

            migrationBuilder.DropColumn(
                name: "activation_id",
                table: "game");

            migrationBuilder.AddColumn<int>(
                name: "activation_id",
                table: "key",
                type: "int",
                nullable: true,
                defaultValueSql: "NULL");

            migrationBuilder.CreateIndex(
                name: "activation_id",
                table: "key",
                column: "activation_id");

            migrationBuilder.CreateIndex(
                name: "name2",
                table: "game",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_key_activation_id",
                table: "key",
                column: "activation_id",
                principalTable: "activation",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
