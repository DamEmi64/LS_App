using Base;
using Base.Connect;
using Invoices.Domain.Repositories;
using Invoices.Infrastructure.Models;
using Razor.Templating.Core;
using Invoices.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Invoices.Domain.Dictionaries;

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
                    Items = invoice.Positions.Where(x=>x.Status != InvoiceStatus.Paid).Select(x => new Invoices.Extras.Resources.InvoiceItem { Title = x.Title, Value = x.Value }).ToList(),
                    PaymentMethod = request.Model.PaymentMethod == InvoicePaymentMethod.BlikPhone ? "blik" : "account",
                    Phone = request.Model.CollectorPhoneNumber,
                    AccountNo = request.Model.AccountNumber
                };
                var html = await RazorTemplateEngine.RenderAsync(TemplatePath, resource);
                QuestPDF.Settings.License = LicenseType.Community;
                var pdf = Document.Create(container => container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Content().Column(column =>
                    {
                        column.Item().Text(resource.Title).FontSize(24).Bold();
                        column.Item().Text($"Wystawca: {resource.Collector}");
                        column.Item().Text($"Odbiorca: {resource.Recipient}");
                        foreach (var item in resource.Items)
                            column.Item().Text($"{item.Title} — {item.Value:0.00} PLN");
                        column.Item().Text(resource.PaymentInfo);
                    });
                })).GeneratePdf();

                var invoiceDb = await _invoiceRepository.GetWithDetails(invoice.Id);
                ArgumentNullException.ThrowIfNull(invoiceDb);

                await _invoiceRepository.SaveDocument(new InvoiceDocument { Invoice = invoiceDb, Html = html, Pdf = pdf });
            }

        }
    }
}
