using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuoPlay.DataManagement.Migrations
{
    /// <inheritdoc />
    public partial class inittictaktoearea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "board_state",
                table: "tictaktoe_games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "board_state",
                table: "tictaktoe_games");
        }
    }
}
