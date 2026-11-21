using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassApp.Migrations
{
    /// <inheritdoc />
    public partial class Refacs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoIncludeRaucherpausen",
                table: "CoiffeurSpielrundeTable");

            migrationBuilder.DropColumn(
                name: "DoIncludeShots",
                table: "CoiffeurSpielrundeTable");

            migrationBuilder.AddColumn<bool>(
                name: "HasRoundedPunkte",
                table: "CoiffeurSpielrundeTable",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasRoundedPunkte",
                table: "CoiffeurSpielrundeTable");

            migrationBuilder.AddColumn<bool>(
                name: "DoIncludeRaucherpausen",
                table: "CoiffeurSpielrundeTable",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DoIncludeShots",
                table: "CoiffeurSpielrundeTable",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
