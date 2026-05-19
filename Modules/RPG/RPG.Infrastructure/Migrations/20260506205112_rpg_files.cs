using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rpg_files : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RPGFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InsBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RPGFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RPGFile_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RPGFile_StoryId",
                table: "RPGFile",
                column: "StoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RPGFile");
        }
    }
}
