using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Automation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _userRef_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Trigger",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Trigger",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "AutomatTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "AutomatTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Automats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Automats",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Trigger");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Trigger");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "AutomatTasks");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "AutomatTasks");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Automats");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Automats");
        }
    }
}
