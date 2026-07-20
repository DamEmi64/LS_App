using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Automation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class automation_event_approach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trigger_Automats_AutomatId",
                table: "Trigger");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trigger",
                table: "Trigger");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Trigger");

            migrationBuilder.RenameTable(
                name: "Trigger",
                newName: "Triggers");

            migrationBuilder.RenameIndex(
                name: "IX_Trigger_AutomatId",
                table: "Triggers",
                newName: "IX_Triggers_AutomatId");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Triggers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Cron",
                table: "Triggers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Triggers",
                table: "Triggers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_Automats_AutomatId",
                table: "Triggers",
                column: "AutomatId",
                principalTable: "Automats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_Automats_AutomatId",
                table: "Triggers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Triggers",
                table: "Triggers");

            migrationBuilder.RenameTable(
                name: "Triggers",
                newName: "Trigger");

            migrationBuilder.RenameIndex(
                name: "IX_Triggers_AutomatId",
                table: "Trigger",
                newName: "IX_Trigger_AutomatId");

            migrationBuilder.AlterColumn<string>(
                name: "EventId",
                table: "Trigger",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Cron",
                table: "Trigger",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Trigger",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trigger",
                table: "Trigger",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trigger_Automats_AutomatId",
                table: "Trigger",
                column: "AutomatId",
                principalTable: "Automats",
                principalColumn: "Id");
        }
    }
}
