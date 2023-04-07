using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.DAL.Migrations
{
    public partial class ChangeTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_order");

            migrationBuilder.RenameColumn(
                name: "path",
                table: "image",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "avatar_path",
                table: "game",
                newName: "avatar_name");

            migrationBuilder.CreateTable(
                name: "key_order",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false),
                    key_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Primary_Key_Order", x => new { x.order_id, x.key_id });
                    table.ForeignKey(
                        name: "FK_key_order_key_id",
                        column: x => x.key_id,
                        principalTable: "key",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_key_order_order_id",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_key_order_key_id",
                table: "key_order",
                column: "key_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "key_order");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "image",
                newName: "path");

            migrationBuilder.RenameColumn(
                name: "avatar_name",
                table: "game",
                newName: "avatar_path");

            migrationBuilder.CreateTable(
                name: "game_order",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false),
                    game_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Primary_Game_Order", x => new { x.order_id, x.game_id });
                    table.ForeignKey(
                        name: "FK_game_order_game_id",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_order_order_id",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_game_order_game_id",
                table: "game_order",
                column: "game_id");
        }
    }
}
