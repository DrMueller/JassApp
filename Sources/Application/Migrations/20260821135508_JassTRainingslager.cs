using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassApp.Migrations
{
    /// <inheritdoc />
    public partial class JassTRainingslager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsJassTrainingslager",
                table: "CoiffeurSpielrundeTable",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsJassTrainingslager",
                table: "CoiffeurSpielrundeTable");
        }
    }
}
