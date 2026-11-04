using Base;
using Base.Connect;
using Invoices.Domain.Repositories;
using Invoices.Infrastructure.Models;
using Razor.Templating.Core;
using Invoices.Domain.Entities;
using Invoices.Domain.Dictionaries;
using Base.Helpers;

namespace Invoices.Infrastructure.Jobs
{
    public class GenerateInvoiceDocument
    {
        public class Job : IJob
        {
            public int OperationId => Operations.GenerateInvoice;

            public Guid Id { get; set; } = Guid.NewGuid();

            public List<IJob> Children => [];

            public string Name => $"Generate Invoice {Model.Invoice.Title}";

            public required InvoiceDocumentModel Model { get; set; }
            public bool IsPdf { get; set; }
        }

        public class Handler : JobHandler<Job>
        {
            private const string TemplatePath = "/Views/Invoice.cshtml";

            private readonly IInvoiceRepository _invoiceRepository;

            public Handler(IJobContext jobContext, IInvoiceRepository invoiceRepository) : base(jobContext)
            {
                _invoiceRepository = invoiceRepository;
            }

            public override async Task Execute(Job request)
            {
                ArgumentNullException.ThrowIfNull(request.Model);
                var invoice = request.Model.Invoice;
                var resource = new Invoices.Extras.Resources.InvoiceDocument
                {
                    Title = invoice.Title,
                    Collector = invoice.Collector.Login ?? invoice.Collector.UserId,
                    Recipient = invoice.Recipient.Login ?? invoice.Recipient.UserId,
                    Items = invoice.Positions.Where(x=>x.Status != InvoiceStatus.Paid).Select(x => new Invoices.Extras.Resources.InvoiceItem { Title = x.Title, Value = x.Value, InvoiceDate = x.InvoiceDate }).ToList(),
                    PaymentMethod = request.Model.PaymentMethod == InvoicePaymentMethod.BlikPhone ? "blik" : "account",
                    Phone = request.Model.CollectorPhoneNumber,
                    AccountNo = request.Model.AccountNumber
                };
                var html = await RazorTemplateEngine.RenderAsync(TemplatePath, resource);
                var pdf = await HtmlPdfRenderer.RenderAsync(html, 15);

                var invoiceDb = await _invoiceRepository.GetWithDetails(invoice.Id);
                ArgumentNullException.ThrowIfNull(invoiceDb);

                await _invoiceRepository.SaveDocument(new InvoiceDocument { Invoice = invoiceDb, Html = html, Pdf = pdf });
            }

        }
    }
}
