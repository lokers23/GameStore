using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "developer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_developer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "platform",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "publisher",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publisher", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accessrole = table.Column<int>(name: "access_role", type: "int", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    developerid = table.Column<int>(name: "developer_id", type: "int", nullable: true, defaultValueSql: "NULL"),
                    publisherid = table.Column<int>(name: "publisher_id", type: "int", nullable: true, defaultValueSql: "NULL"),
                    releaseon = table.Column<DateTime>(name: "release_on", type: "date", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValueSql: "NULL"),
                    price = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    videourl = table.Column<string>(name: "video_url", type: "nvarchar(1000)", maxLength: 1000, nullable: true, defaultValueSql: "NULL"),
                    avatarpath = table.Column<string>(name: "avatar_path", type: "nvarchar(1000)", maxLength: 1000, nullable: true, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_developer_id",
                        column: x => x.developerid,
                        principalTable: "developer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_game_publisher_id",
                        column: x => x.publisherid,
                        principalTable: "publisher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    mail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    balance = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    roleid = table.Column<int>(name: "role_id", type: "int", nullable: true, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_id",
                        column: x => x.roleid,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "game_genre",
                columns: table => new
                {
                    gameid = table.Column<int>(name: "game_id", type: "int", nullable: false),
                    genreid = table.Column<int>(name: "genre_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Primary_Game_Genre", x => new { x.gameid, x.genreid });
                    table.ForeignKey(
                        name: "FK_game_genre_game_id",
                        column: x => x.gameid,
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_genre_genre_id",
                        column: x => x.genreid,
                        principalTable: "genre",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "key",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gameid = table.Column<int>(name: "game_id", type: "int", nullable: true, defaultValueSql: "NULL"),
                    activationid = table.Column<int>(name: "activation_id", type: "int", nullable: true, defaultValueSql: "NULL"),
                    isused = table.Column<bool>(name: "is_used", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_key", x => x.id);
                    table.ForeignKey(
                        name: "FK_key_activation_id",
                        column: x => x.activationid,
                        principalTable: "activation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_key_game_id",
                        column: x => x.gameid,
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "minimum_specification",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    os = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    processor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    memory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    storage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    graphics = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    platformid = table.Column<int>(name: "platform_id", type: "int", nullable: false),
                    gameid = table.Column<int>(name: "game_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_minimum_specification", x => x.id);
                    table.ForeignKey(
                        name: "FK_minimum_specification_game_id",
                        column: x => x.gameid,
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_minimum_specification_platform_id",
                        column: x => x.platformid,
                        principalTable: "platform",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    payon = table.Column<DateTime>(name: "pay_on", type: "date", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    userid = table.Column<int>(name: "user_id", type: "int", nullable: true, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_user_id",
                        column: x => x.userid,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "game_order",
                columns: table => new
                {
                    orderid = table.Column<int>(name: "order_id", type: "int", nullable: false),
                    gameid = table.Column<int>(name: "game_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Primary_Game_Order", x => new { x.orderid, x.gameid });
                    table.ForeignKey(
                        name: "FK_game_order_game_id",
                        column: x => x.gameid,
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_order_order_id",
                        column: x => x.orderid,
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
                name: "name",
                table: "developer",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "developer_id",
                table: "game",
                column: "developer_id");

            migrationBuilder.CreateIndex(
                name: "name",
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
                name: "name",
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
                name: "game_id",
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
                name: "name",
                table: "platform",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "name",
                table: "publisher",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "access_role",
                table: "role",
                column: "access_role",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "role_id",
                table: "user",
                column: "role_id");
        }

        /// <inheritdoc />
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

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
