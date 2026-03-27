using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Automation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Automats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastRun = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    InsDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Automats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutomatTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutomatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomatTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomatTasks_Automats_AutomatId",
                        column: x => x.AutomatId,
                        principalTable: "Automats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trigger",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Cron = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutomatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trigger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trigger_Automats_AutomatId",
                        column: x => x.AutomatId,
                        principalTable: "Automats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutomatTasks_AutomatId",
                table: "AutomatTasks",
                column: "AutomatId");

            migrationBuilder.CreateIndex(
                name: "IX_Trigger_AutomatId",
                table: "Trigger",
                column: "AutomatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutomatTasks");

            migrationBuilder.DropTable(
                name: "Trigger");

            migrationBuilder.DropTable(
                name: "Automats");
        }
    }
}
