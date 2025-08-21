using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassApp.Migrations
{
    /// <inheritdoc />
    public partial class Spielrunde : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoiffeurSpielrundeTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoiffeurSpielrundeTyp = table.Column<int>(type: "int", nullable: false),
                    GestartetAm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Punktewert = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoiffeurSpielrundeTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JassTeamTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoiffeurSpielrundeId = table.Column<int>(type: "int", nullable: false),
                    JassTeamTyp = table.Column<int>(type: "int", nullable: false),
                    Spieler1Id = table.Column<int>(type: "int", nullable: false),
                    Spieler2Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JassTeamTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JassTeamTable_CoiffeurSpielrundeTable_CoiffeurSpielrundeId",
                        column: x => x.CoiffeurSpielrundeId,
                        principalTable: "CoiffeurSpielrundeTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JassTeamTable_SpielerTable_Spieler1Id",
                        column: x => x.Spieler1Id,
                        principalTable: "SpielerTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JassTeamTable_SpielerTable_Spieler2Id",
                        column: x => x.Spieler2Id,
                        principalTable: "SpielerTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrumpfrundeTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoiffeurSpielrundeId = table.Column<int>(type: "int", nullable: false),
                    PunkteModifikator = table.Column<int>(type: "int", nullable: false),
                    ResultatTeam1 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ResultatTeam2 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TrumpfTyp = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrumpfrundeTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrumpfrundeTable_CoiffeurSpielrundeTable_CoiffeurSpielrundeId",
                        column: x => x.CoiffeurSpielrundeId,
                        principalTable: "CoiffeurSpielrundeTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JassTeamTable_CoiffeurSpielrundeId",
                table: "JassTeamTable",
                column: "CoiffeurSpielrundeId");

            migrationBuilder.CreateIndex(
                name: "IX_JassTeamTable_Spieler1Id",
                table: "JassTeamTable",
                column: "Spieler1Id");

            migrationBuilder.CreateIndex(
                name: "IX_JassTeamTable_Spieler2Id",
                table: "JassTeamTable",
                column: "Spieler2Id");

            migrationBuilder.CreateIndex(
                name: "IX_TrumpfrundeTable_CoiffeurSpielrundeId",
                table: "TrumpfrundeTable",
                column: "CoiffeurSpielrundeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JassTeamTable");

            migrationBuilder.DropTable(
                name: "TrumpfrundeTable");

            migrationBuilder.DropTable(
                name: "CoiffeurSpielrundeTable");
        }
    }
}
