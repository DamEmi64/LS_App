using Events.Infrastructure.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastructure.Migrations
{
    [DbContext(typeof(EventContext))]
    [Migration("20261007000000_EventInvoices")]
    partial class EventInvoices
    {
    }
}
