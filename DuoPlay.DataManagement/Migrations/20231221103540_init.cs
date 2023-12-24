using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuoPlay.DataManagement.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "playareas",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_player = table.Column<long>(type: "bigint", nullable: false),
                    playarea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    confirmed_playarea = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("playareas_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("players_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ships",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_playarea = table.Column<long>(type: "bigint", nullable: false),
                    length = table.Column<long>(type: "bigint", nullable: false),
                    decks_json = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ships_pkey", x => x.id);
                    table.ForeignKey(
                        name: "ships_id_playarea",
                        column: x => x.id_playarea,
                        principalTable: "playareas",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_player_host = table.Column<long>(type: "bigint", nullable: false),
                    id_player_join = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    start_session = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    end_session = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sessions_pkey", x => x.id);
                    table.ForeignKey(
                        name: "sessions_id_player_host_foreign",
                        column: x => x.id_player_host,
                        principalTable: "players",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "sessions_id_player_join_foreign",
                        column: x => x.id_player_join,
                        principalTable: "players",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "seabattle_games",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_player_turn = table.Column<long>(type: "bigint", nullable: true),
                    id_player_win = table.Column<long>(type: "bigint", nullable: true),
                    id_session = table.Column<long>(type: "bigint", nullable: false),
                    game_message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_game = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    end_game = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("seabattle_games_pkey", x => x.id);
                    table.ForeignKey(
                        name: "seabattle_games_id_player_turn_foreign",
                        column: x => x.id_player_turn,
                        principalTable: "players",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "seabattle_games_id_player_win_foreign",
                        column: x => x.id_player_win,
                        principalTable: "players",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "seabattle_games_id_session_foreign",
                        column: x => x.id_session,
                        principalTable: "sessions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tictaktoe_games",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_player_turn = table.Column<long>(type: "bigint", nullable: true),
                    id_player_win = table.Column<long>(type: "bigint", nullable: true),
                    id_session = table.Column<long>(type: "bigint", nullable: false),
                    game_message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_game = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    end_game = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tictaktoe_games_pkey", x => x.id);
                    table.ForeignKey(
                        name: "tictaktoe_games_id_player_turn_foreign",
                        column: x => x.id_player_turn,
                        principalTable: "players",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "tictaktoe_games_id_player_win_foreign",
                        column: x => x.id_player_win,
                        principalTable: "players",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "tictaktoe_games_id_session_foreign",
                        column: x => x.id_session,
                        principalTable: "sessions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "shoots",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_seabattle_game = table.Column<long>(type: "bigint", nullable: false),
                    id_player_shoot = table.Column<long>(type: "bigint", nullable: false),
                    shoot_coordinate_Y = table.Column<long>(type: "bigint", nullable: false),
                    shoot_coordinate_X = table.Column<long>(type: "bigint", nullable: false),
                    time_shoot = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("shoots_pkey", x => x.id);
                    table.ForeignKey(
                        name: "shoots_id_player_shoot_foreign",
                        column: x => x.id_player_shoot,
                        principalTable: "players",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "shoots_id_seabattle_game_foreign",
                        column: x => x.id_seabattle_game,
                        principalTable: "seabattle_games",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "players_name_unique",
                table: "players",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_seabattle_games_id_player_turn",
                table: "seabattle_games",
                column: "id_player_turn");

            migrationBuilder.CreateIndex(
                name: "IX_seabattle_games_id_player_win",
                table: "seabattle_games",
                column: "id_player_win");

            migrationBuilder.CreateIndex(
                name: "seabattle_games_id_session_unique",
                table: "seabattle_games",
                column: "id_session",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessions_id_player_host",
                table: "sessions",
                column: "id_player_host");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_id_player_join",
                table: "sessions",
                column: "id_player_join");

            migrationBuilder.CreateIndex(
                name: "sessions_name_unique",
                table: "sessions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ships_id_playarea",
                table: "ships",
                column: "id_playarea");

            migrationBuilder.CreateIndex(
                name: "IX_shoots_id_player_shoot",
                table: "shoots",
                column: "id_player_shoot");

            migrationBuilder.CreateIndex(
                name: "shoots_id_seabattle_game_unique",
                table: "shoots",
                column: "id_seabattle_game",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tictaktoe_games_id_player_turn",
                table: "tictaktoe_games",
                column: "id_player_turn");

            migrationBuilder.CreateIndex(
                name: "IX_tictaktoe_games_id_player_win",
                table: "tictaktoe_games",
                column: "id_player_win");

            migrationBuilder.CreateIndex(
                name: "tictaktoe_games_id_session_unique",
                table: "tictaktoe_games",
                column: "id_session",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ships");

            migrationBuilder.DropTable(
                name: "shoots");

            migrationBuilder.DropTable(
                name: "tictaktoe_games");

            migrationBuilder.DropTable(
                name: "playareas");

            migrationBuilder.DropTable(
                name: "seabattle_games");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
