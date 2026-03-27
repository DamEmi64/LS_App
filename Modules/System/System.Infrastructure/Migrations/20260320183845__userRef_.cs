using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _userRef_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_UserDatas_UserId",
                table: "Processes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDatas",
                table: "UserDatas");

            migrationBuilder.RenameTable(
                name: "UserDatas",
                newName: "UserData");

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Processes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Processes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "ProcessErrors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "ProcessErrors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Milestones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Milestones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Media",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Media",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserData",
                table: "UserData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_UserData_UserId",
                table: "Processes",
                column: "UserId",
                principalTable: "UserData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Processes_UserData_UserId",
                table: "Processes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserData",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "ProcessErrors");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "ProcessErrors");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Jobs");

            migrationBuilder.RenameTable(
                name: "UserData",
                newName: "UserDatas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDatas",
                table: "UserDatas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_UserDatas_UserId",
                table: "Processes",
                column: "UserId",
                principalTable: "UserDatas",
                principalColumn: "Id");
        }
    }
}
