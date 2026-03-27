using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Communication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _userRef_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsBy",
                table: "Emails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdBy",
                table: "Emails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "InsBy",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "UpdBy",
                table: "Emails");
        }
    }
}
