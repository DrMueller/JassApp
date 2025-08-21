using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassApp.Migrations
{
    /// <inheritdoc />
    public partial class JassTeamSpieler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JassTeamTable_SpielerTable_Spieler1Id",
                table: "JassTeamTable");

            migrationBuilder.DropForeignKey(
                name: "FK_JassTeamTable_SpielerTable_Spieler2Id",
                table: "JassTeamTable");

            migrationBuilder.RenameColumn(
                name: "Spieler2Id",
                table: "JassTeamTable",
                newName: "JassTeamSpieler2Id");

            migrationBuilder.RenameColumn(
                name: "Spieler1Id",
                table: "JassTeamTable",
                newName: "JassTeamSpieler1Id");

            migrationBuilder.RenameIndex(
                name: "IX_JassTeamTable_Spieler2Id",
                table: "JassTeamTable",
                newName: "IX_JassTeamTable_JassTeamSpieler2Id");

            migrationBuilder.RenameIndex(
                name: "IX_JassTeamTable_Spieler1Id",
                table: "JassTeamTable",
                newName: "IX_JassTeamTable_JassTeamSpieler1Id");

            migrationBuilder.CreateTable(
                name: "JassTeamSpielerTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IstStartSpieler = table.Column<bool>(type: "bit", nullable: false),
                    JassTeamId = table.Column<int>(type: "int", nullable: false),
                    SpielerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JassTeamSpielerTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JassTeamSpielerTable_JassTeamTable_JassTeamId",
                        column: x => x.JassTeamId,
                        principalTable: "JassTeamTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JassTeamSpielerTable_SpielerTable_SpielerId",
                        column: x => x.SpielerId,
                        principalTable: "SpielerTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JassTeamSpielerTable_JassTeamId",
                table: "JassTeamSpielerTable",
                column: "JassTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_JassTeamSpielerTable_SpielerId",
                table: "JassTeamSpielerTable",
                column: "SpielerId");

            migrationBuilder.AddForeignKey(
                name: "FK_JassTeamTable_JassTeamSpielerTable_JassTeamSpieler1Id",
                table: "JassTeamTable",
                column: "JassTeamSpieler1Id",
                principalTable: "JassTeamSpielerTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JassTeamTable_JassTeamSpielerTable_JassTeamSpieler2Id",
                table: "JassTeamTable",
                column: "JassTeamSpieler2Id",
                principalTable: "JassTeamSpielerTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JassTeamTable_JassTeamSpielerTable_JassTeamSpieler1Id",
                table: "JassTeamTable");

            migrationBuilder.DropForeignKey(
                name: "FK_JassTeamTable_JassTeamSpielerTable_JassTeamSpieler2Id",
                table: "JassTeamTable");

            migrationBuilder.DropTable(
                name: "JassTeamSpielerTable");

            migrationBuilder.RenameColumn(
                name: "JassTeamSpieler2Id",
                table: "JassTeamTable",
                newName: "Spieler2Id");

            migrationBuilder.RenameColumn(
                name: "JassTeamSpieler1Id",
                table: "JassTeamTable",
                newName: "Spieler1Id");

            migrationBuilder.RenameIndex(
                name: "IX_JassTeamTable_JassTeamSpieler2Id",
                table: "JassTeamTable",
                newName: "IX_JassTeamTable_Spieler2Id");

            migrationBuilder.RenameIndex(
                name: "IX_JassTeamTable_JassTeamSpieler1Id",
                table: "JassTeamTable",
                newName: "IX_JassTeamTable_Spieler1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JassTeamTable_SpielerTable_Spieler1Id",
                table: "JassTeamTable",
                column: "Spieler1Id",
                principalTable: "SpielerTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JassTeamTable_SpielerTable_Spieler2Id",
                table: "JassTeamTable",
                column: "Spieler2Id",
                principalTable: "SpielerTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
