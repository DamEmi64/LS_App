using System;
using Invoices.Infrastructure.Db;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Invoices.Infrastructure.Migrations
{
    [DbContext(typeof(InvoiceContext))]
    [Migration("20261007090000_AddInvoiceDateToInvoicePositions")]
    public partial class AddInvoiceDateToInvoicePositions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InvoiceDate",
                table: "InvoicePositions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "InvoiceDate", table: "InvoicePositions");
        }
    }
}
