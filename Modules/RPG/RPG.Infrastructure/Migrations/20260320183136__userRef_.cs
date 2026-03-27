using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _userRef_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Stories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Stories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Skill",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Skill",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Session",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Session",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "PlayerData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "PlayerData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Link",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Link",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Heroes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Heroes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Chapters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Chapters",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "PlayerData");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "PlayerData");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Link");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Link");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Heroes");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Heroes");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Chapters");
        }
    }
}
