using Base;
using Invoices.Infrastructure.Db;
using Invoices.Domain.Repositories;
using Invoices.Infrastructure.Repositories;
using Invoices.Infrastructure.Services.InvoiceDocumentService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Invoices.Application
{
    public class InvoiceModule : IModule
    {
        public IEnumerable<Operation> Operations => new List<Operation>
        {
            Extensions.Operation(Domain.Dictionaries.Operations.GenerateInvoice,"Generate invoice","gen_invoice"),
        };

        public string Name => "Invoices";

        public string Version => "v0.1";

        public IEnumerable<PermissionInfo> Permissions => [PermissionInfo.Create("invoices", "Create and manage invoices", true)];

        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddDatabase<InvoiceContext>(AppConfiguration.DefaultConnectionString);
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IInvoiceDocumentService, InvoiceDocumentService>();
            return services;
        }

        public IApplicationBuilder OnStartup(IApplicationBuilder app)
        {
            return app;
        }
    }
}
