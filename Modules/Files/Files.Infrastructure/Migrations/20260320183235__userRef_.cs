using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Files.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _userRef_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "AdditionalData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "AdditionalData",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "AdditionalData");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "AdditionalData");
        }
    }
}
