using Events.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastructure.Migrations
{
    [DbContext(typeof(EventContext))]
    [Migration("20261125000000_CircularEventRecurrence")]
    public partial class CircularEventRecurrence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CircularPeriod",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CircularStartDate",
                table: "Events",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "CircularPeriod", table: "Events");
            migrationBuilder.DropColumn(name: "CircularStartDate", table: "Events");
        }
    }
}