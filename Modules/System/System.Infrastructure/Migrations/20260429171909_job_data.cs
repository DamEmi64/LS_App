using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace System.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class job_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Schema",
                table: "Processes");

            migrationBuilder.DropColumn(
                name: "JobData",
                table: "Jobs");

            migrationBuilder.AddColumn<Guid>(
                name: "JobDataId",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InsBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobDataId",
                table: "Jobs",
                column: "JobDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobData_JobDataId",
                table: "Jobs",
                column: "JobDataId",
                principalTable: "JobData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobData_JobDataId",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "JobData");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobDataId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobDataId",
                table: "Jobs");

            migrationBuilder.AddColumn<string>(
                name: "Schema",
                table: "Processes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobData",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
