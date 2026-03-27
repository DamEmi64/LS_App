using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Files.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameGenre = table.Column<int>(type: "int", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Semester = table.Column<int>(type: "int", nullable: true),
                    InsDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Locaction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    AdditionalDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_AdditionalData_AdditionalDataId",
                        column: x => x.AdditionalDataId,
                        principalTable: "AdditionalData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceType = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imported = table.Column<bool>(type: "bit", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sources_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_AdditionalDataId",
                table: "Files",
                column: "AdditionalDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_FileId",
                table: "Sources",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "AdditionalData");
        }
    }
}
