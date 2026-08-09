using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class meeting_summary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Session",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Session");
        }
    }
}
