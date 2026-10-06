using Base;
using Base.Connect;
using Invoices.Infrastructure.Models;
using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invoices.Infrastructure.Jobs
{
    public class GenerateInvoiceDocument
    {
        public class Job : IJob
        {

            public int OperationId => throw new NotImplementedException();

            public Guid Id { get; set; } = Guid.NewGuid();

            public List<IJob> Children => [];

            public string Name => throw new NotImplementedException();

            public required InvoiceDocumentModel Model { get; set; }
            public bool IsPdf { get; set; }
        }

        public class Handler : JobHandler<Job>
        {
            private const string TemplatePath = "/Views/Invoice.cshtml";

            public override async Task Execute(Job request)
            {
                if (request.IsPdf)
                {
                    await GeneratePdf(request.Model);
                }
                else
                {
                    await GenerateHtml(request.Model);
                }
            }

            private async Task<string> GenerateHtml(InvoiceDocumentModel model)
            {
                ArgumentNullException.ThrowIfNull(model);
                model.Validate();

                return await RazorTemplateEngine.RenderAsync(TemplatePath, model);
            }

            private async Task<string> GeneratePdf(InvoiceDocumentModel model)
            {
                ArgumentNullException.ThrowIfNull(model);
                model.Validate();

                return await RazorTemplateEngine.RenderAsync(TemplatePath, model);
            }
        }
    }
}
