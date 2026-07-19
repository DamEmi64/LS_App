using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class drive_context : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Media",
                newName:"Media_old");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ContentStr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });
        }
    }
}
