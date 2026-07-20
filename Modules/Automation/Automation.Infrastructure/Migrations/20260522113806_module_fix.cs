using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Automation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class module_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutomatTasks_Automats_AutomatId",
                table: "AutomatTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_AutomatTasks_Automats_AutomatId",
                table: "AutomatTasks",
                column: "AutomatId",
                principalTable: "Automats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutomatTasks_Automats_AutomatId",
                table: "AutomatTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_AutomatTasks_Automats_AutomatId",
                table: "AutomatTasks",
                column: "AutomatId",
                principalTable: "Automats",
                principalColumn: "Id");
        }
    }
}
