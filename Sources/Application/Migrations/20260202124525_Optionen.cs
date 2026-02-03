using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassApp.Migrations
{
    /// <inheritdoc />
    public partial class Optionen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoIncludeRaucherpausen",
                table: "CoiffeurSpielrundeTable");

            migrationBuilder.DropColumn(
                name: "DoIncludeShots",
                table: "CoiffeurSpielrundeTable");
        }
    }
}
