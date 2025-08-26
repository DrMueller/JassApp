using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassApp.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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
                name: "SpielerTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpielerTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JassTeamTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoiffeurSpielrundeId = table.Column<int>(type: "int", nullable: false),
                    JassTeamTyp = table.Column<int>(type: "int", nullable: false)
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
                    CoiffeurTrumpfTyp = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "JassTeamSpielerTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IstStartSpieler = table.Column<bool>(type: "bit", nullable: false),
                    JassTeamId = table.Column<int>(type: "int", nullable: false),
                    SpielerId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JassTeamSpielerTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JassTeamSpielerTable_JassTeamTable_JassTeamId",
                        column: x => x.JassTeamId,
                        principalTable: "JassTeamTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_JassTeamTable_CoiffeurSpielrundeId",
                table: "JassTeamTable",
                column: "CoiffeurSpielrundeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrumpfrundeTable_CoiffeurSpielrundeId",
                table: "TrumpfrundeTable",
                column: "CoiffeurSpielrundeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JassTeamSpielerTable");

            migrationBuilder.DropTable(
                name: "TrumpfrundeTable");

            migrationBuilder.DropTable(
                name: "JassTeamTable");

            migrationBuilder.DropTable(
                name: "SpielerTable");

            migrationBuilder.DropTable(
                name: "CoiffeurSpielrundeTable");
        }
    }
}
