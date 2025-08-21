using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassApp.Migrations
{
    /// <inheritdoc />
    public partial class JassTeamSpieler2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JassTeamSpielerTable_JassTeamTable_JassTeamId",
                table: "JassTeamSpielerTable");

            migrationBuilder.AddForeignKey(
                name: "FK_JassTeamSpielerTable_JassTeamTable_JassTeamId",
                table: "JassTeamSpielerTable",
                column: "JassTeamId",
                principalTable: "JassTeamTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JassTeamSpielerTable_JassTeamTable_JassTeamId",
                table: "JassTeamSpielerTable");

            migrationBuilder.AddForeignKey(
                name: "FK_JassTeamSpielerTable_JassTeamTable_JassTeamId",
                table: "JassTeamSpielerTable",
                column: "JassTeamId",
                principalTable: "JassTeamTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
