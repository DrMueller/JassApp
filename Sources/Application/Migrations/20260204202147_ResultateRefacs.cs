using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassApp.Migrations
{
    /// <inheritdoc />
    public partial class ResultateRefacs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ResultatTeam2",
                table: "TrumpfrundeTable",
                type: "int",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResultatTeam1",
                table: "TrumpfrundeTable",
                type: "int",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IstKonterMatchTeam1",
                table: "TrumpfrundeTable",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IstKonterMatchTeam2",
                table: "TrumpfrundeTable",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IstMatschTeam1",
                table: "TrumpfrundeTable",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IstMatschTeam2",
                table: "TrumpfrundeTable",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IstKonterMatchTeam1",
                table: "TrumpfrundeTable");

            migrationBuilder.DropColumn(
                name: "IstKonterMatchTeam2",
                table: "TrumpfrundeTable");

            migrationBuilder.DropColumn(
                name: "IstMatschTeam1",
                table: "TrumpfrundeTable");

            migrationBuilder.DropColumn(
                name: "IstMatschTeam2",
                table: "TrumpfrundeTable");

            migrationBuilder.AlterColumn<string>(
                name: "ResultatTeam2",
                table: "TrumpfrundeTable",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResultatTeam1",
                table: "TrumpfrundeTable",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 10,
                oldNullable: true);
        }
    }
}
