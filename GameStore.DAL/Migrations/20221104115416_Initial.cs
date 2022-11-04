using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace GameStore.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "developer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_developer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "platform",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "publisher",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publisher", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    login = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    mail = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    balance = table.Column<decimal>(type: "decimal(20,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    developer_id = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "NULL"),
                    publisher_id = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "NULL"),
                    release_on = table.Column<DateTime>(type: "date", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true, defaultValueSql: "NULL"),
                    price = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    video_url = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, defaultValueSql: "NULL"),
                    avatar_path = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_developer_id",
                        column: x => x.developer_id,
                        principalTable: "developer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_game_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "publisher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    pay_on = table.Column<DateTime>(type: "date", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    user_id = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "game_genre",
                columns: table => new
                {
                    game_id = table.Column<int>(type: "int(11)", nullable: false),
                    genre_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.game_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_game_genre_game_id",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_genre_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genre",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "key",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    value = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    game_id = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "NULL"),
                    activation_id = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "NULL"),
                    is_used = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_key", x => x.id);
                    table.ForeignKey(
                        name: "FK_key_activation_id",
                        column: x => x.activation_id,
                        principalTable: "activation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_key_game_id",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "minimum_specification",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    os = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    processor = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    memory = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    storage = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    graphics = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    platform_id = table.Column<int>(type: "int(11)", nullable: false),
                    game_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_minimum_specification", x => x.id);
                    table.ForeignKey(
                        name: "FK_minimum_specification_game_id",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_minimum_specification_platform_id",
                        column: x => x.platform_id,
                        principalTable: "platform",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "game_order",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int(11)", nullable: false),
                    game_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.order_id, x.game_id });
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
                name: "name",
                table: "activation",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "name1",
                table: "developer",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "developer_id",
                table: "game",
                column: "developer_id");

            migrationBuilder.CreateIndex(
                name: "name2",
                table: "game",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "publisher_id",
                table: "game",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_genre_genre_id",
                table: "game_genre",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_order_game_id",
                table: "game_order",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "name3",
                table: "genre",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "activation_id",
                table: "key",
                column: "activation_id");

            migrationBuilder.CreateIndex(
                name: "game_id",
                table: "key",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "value",
                table: "key",
                column: "value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "game_id1",
                table: "minimum_specification",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "platform_id_game_id",
                table: "minimum_specification",
                columns: new[] { "platform_id", "game_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_id",
                table: "order",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "name4",
                table: "platform",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "name5",
                table: "publisher",
                column: "name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_genre");

            migrationBuilder.DropTable(
                name: "game_order");

            migrationBuilder.DropTable(
                name: "key");

            migrationBuilder.DropTable(
                name: "minimum_specification");

            migrationBuilder.DropTable(
                name: "genre");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "activation");

            migrationBuilder.DropTable(
                name: "game");

            migrationBuilder.DropTable(
                name: "platform");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "developer");

            migrationBuilder.DropTable(
                name: "publisher");
        }
    }
}
